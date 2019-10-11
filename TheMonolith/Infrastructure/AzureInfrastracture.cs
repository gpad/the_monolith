using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ServiceBus.Fluent;

namespace lb
{
    class AzureInfrastracture
    {
        private const string TopicNameLBStatus = "lb_status";
        private const string TopicNameAsUpdates = "as_updates";

        private static IAzure CreateAzure()
        {
            var files = new[] { "lb/my.azureauth", "my.azureauth" };
            var path = files.Where(f => File.Exists(f)).FirstOrDefault();
            var credentials = Microsoft.Azure.Management.ResourceManager.Fluent.SdkContext.AzureCredentialsFactory.FromFile(path);

            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
            return azure;
        }

        public static async Task<AzureInfrastractureInfoForLB> CreateInfrastructureForLBAsync(string namespaceName, string id)
        {
            IAzure azure = CreateAzure();

            IServiceBusNamespace nameSpace = await RequireNamespace(azure, namespaceName);
            var topic = await FindOrCreateTopic(nameSpace, TopicNameLBStatus);
            await FindOrCreateSubscriptionAsync(topic, SubscriptionName(id));

            var asTopic = await RequireTopic(nameSpace, TopicNameAsUpdates);
            await FindOrCreateSubscriptionAsync(asTopic, SubscriptionName(id));

            return new AzureInfrastractureInfoForLB(TopicNameLBStatus, SubscriptionName(id), TopicNameAsUpdates, SubscriptionName(id));
        }

        private static async Task<ITopic> RequireTopic(IServiceBusNamespace nameSpace, string topicName)
        {
            var topics = await nameSpace.Topics.ListAsync();
            var topic = topics.Where(t => t.Name == topicName).FirstOrDefault();
            if (topic == null)
                throw new ArgumentException($"Unable to find topic {topicName}");
            return topic;
        }

        internal static async Task<AzureInfrastractureInfoForAS> CreateInfrastructureForASAsync(string namespaceName)
        {
            var azure = CreateAzure();

            var nameSpace = await RequireNamespace(azure, namespaceName).ConfigureAwait(false);

            var topic = await FindOrCreateTopic(nameSpace, TopicNameAsUpdates);

            return new AzureInfrastractureInfoForAS(TopicNameAsUpdates);
        }

        private static async Task<ISubscription> FindOrCreateSubscriptionAsync(ITopic topic, string name)
        {
            System.Console.WriteLine("Find or Create Subscription {0}", name);
            var subscriptions = await topic.Subscriptions.ListAsync();
            Dump(subscriptions);

            return await topic.Subscriptions.Define(name).CreateAsync();
        }

        private static string SubscriptionName(string id)
        {
            return $"load_balancer_{id}";
        }

        private static async Task<ITopic> FindOrCreateTopic(IServiceBusNamespace nameSpace, string name)
        {
            System.Console.WriteLine("Find or Create Topic {0}", name);
            var topics = await nameSpace.Topics.ListAsync();
            Dump(topics);

            return await nameSpace.Topics.Define(name).CreateAsync();
        }

        private static async Task<IServiceBusNamespace> RequireNamespace(Microsoft.Azure.Management.Fluent.IAzure azure, string nameSpaceName)
        {
            System.Console.WriteLine("Require namespace -> {0}", nameSpaceName);
            var namespaces = await azure.ServiceBusNamespaces.ListAsync();
            Dump(namespaces);
            var nameSpace = namespaces.Where(ns => ns.Name == nameSpaceName).FirstOrDefault();
            if (nameSpace == null)
                throw new ArgumentException($"Unable to find namespace {nameSpaceName}");
            return nameSpace;
        }

        // private static void Dump<T>(System.Collections.Generic.IEnumerable<T> list)
        // {
        //     list.ToList().ForEach(e => Dump(e));
        // }

        private static void Dump(IPagedCollection<IServiceBusNamespace> namespaces)
        {
            namespaces.ToList().ForEach(Dump);
        }

        private static void Dump(IPagedCollection<ITopic> topics)
        {
            topics.ToList().ForEach(Dump);
        }

        private static void Dump(IPagedCollection<ISubscription> subscriptions)
        {
            subscriptions.ToList().ForEach(Dump);
        }

        private static void Dump(IServiceBusNamespace n)
        {
            System.Console.WriteLine($"\nNamespace\n\tid: {n.Id}\n\tFQDN: {n.Fqdn}\n\tname: {n.Name}\n---\n");
        }

        private static void Dump(ITopic topic)
        {
            System.Console.WriteLine("*** TOPIC ***");
            System.Console.WriteLine($"\tName: {topic.Name}");
            System.Console.WriteLine($"\tId: {topic.Id}");
            System.Console.WriteLine($"\tStatus: {topic.Status}");
            System.Console.WriteLine("---------------");
        }

        private static void Dump(ISubscription subscription)
        {
            System.Console.WriteLine("*** SUBSCRIPTION ***");
            System.Console.WriteLine($"\tName: {subscription.Name}");
            System.Console.WriteLine($"\tId: {subscription.Id}");
            System.Console.WriteLine($"\tStatus: {subscription.Status}");
            System.Console.WriteLine("---------------");
        }

        public class AzureInfrastractureInfoForLB
        {
            public string LBStatusTopicName { get; }
            public string LBStatusSubscriptionName { get; }
            public string ASUpdatesTopicName { get; }
            public string ASUpdatesSubscriptionName { get; }

            public AzureInfrastractureInfoForLB(string lbStatusTopicName, string lbStatusSubscriptionName, string asUpdatesTopicName, string asUpdatesSubscriptionName)
            {
                this.LBStatusTopicName = lbStatusTopicName;
                this.LBStatusSubscriptionName = lbStatusSubscriptionName;
                this.ASUpdatesTopicName = asUpdatesTopicName;
                this.ASUpdatesSubscriptionName = asUpdatesSubscriptionName;
            }
        }

        public class AzureInfrastractureInfoForAS
        {
            public string TopicName { get; }

            public AzureInfrastractureInfoForAS(string topicName)
            {
                this.TopicName = topicName;
            }
        }
    }
}
