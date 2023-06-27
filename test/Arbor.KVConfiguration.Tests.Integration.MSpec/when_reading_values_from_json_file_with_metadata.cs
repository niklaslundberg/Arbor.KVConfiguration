using System;
using System.IO;
using System.Linq;
using Arbor.Aesculus.NCrunch;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.JsonConfiguration;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.MSpec
{
    [Subject(typeof(JsonFileReader))]
    public class when_reading_values_from_json_file_with_metadata
    {
        static string appsettings_full_path = null!;

        static JsonKeyValueConfiguration json_key_value_configuration = null!;

        Establish context =
            () =>
            {
                appsettings_full_path = Path.Combine(
                    VcsTestPathHelper.TryFindVcsRootPath()!,
                    "test",
                    "Arbor.KVConfiguration.Tests.Integration",
                    "appsettings.json");
            };

        Because of = () => json_key_value_configuration = new JsonKeyValueConfiguration(appsettings_full_path);

        It should_have_three_values = () =>
        {
            foreach (MultipleValuesStringPair stringPair in json_key_value_configuration.AllWithMultipleValues)
            {
                Console.WriteLine(stringPair.Key);

                foreach (string value in stringPair.Values)
                {
                    Console.WriteLine("  " + value);
                }
            }

            json_key_value_configuration.AllWithMultipleValues.SelectMany(_ => _.Values).Count().ShouldEqual(3);
        };

        It should_have_two_keys = () =>
        {
            foreach (string key in json_key_value_configuration.AllKeys)
            {
                Console.WriteLine(key);
            }

            json_key_value_configuration.AllKeys.Length.ShouldEqual(2);
        };
    }
}