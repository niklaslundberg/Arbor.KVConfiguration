﻿using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(MultiSourceKeyValueConfiguration))]
    public class when_getting_all_values_from_multisource_where_the_previous_has_value
    {
        private static MultiSourceKeyValueConfiguration _multiSourceKeyValueConfiguration;
        private static ImmutableArray<StringPair> foundValue;

        private Establish context = () =>
        {
            var userKeys = new NameValueCollection
            {
                { "urn:a:complex:immutable:type:instance1:id", "myId1" },
                { "urn:a:complex:immutable:type:instance1:name", "myName1" },
                { "urn:a:complex:immutable:type:instance1:children", "myChild1.1" },
                { "urn:a:complex:immutable:type:instance1:children", "myChild1.2" }
            };

            var baseKeys = new NameValueCollection
            {
                { "urn:unrelated", "true" }
            };

            _multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(baseKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(userKeys))
                .Build();
        };

        private Because of = () =>
            foundValue = _multiSourceKeyValueConfiguration.AllValues;

        private It should_find_the_previous_value = () => foundValue.Select(s => s.Value).ShouldContain("true", "myId1", "myName1", "myChild1.1,myChild1.2");
    }
}