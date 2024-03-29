﻿using Nethereum.Generators;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NethereumGenerator.ExtendedConsole.Configuration
{
    public class ABICollectionConfiguration
    {
        public List<ABIConfiguration> ABIConfigurations { get; set; }

        private static readonly JsonConverter JsonConverter = new StringEnumConverter();

        public IEnumerable<ContractProjectGenerator> GetContractProjectGenerators(string defaultNamespace, string projectFolder)
        {
            return ABIConfigurations.Select(x => x.CreateGenerator(defaultNamespace, projectFolder));
        }

        public void SaveToJson(string outputDirectory, string fileName = null)
        {
            if (fileName == null)
                fileName = GeneratorConfigurationUtils.ConfigFileName;

            var fullPath = Path.Combine(outputDirectory, fileName);

            File.WriteAllText(fullPath, JsonConvert.SerializeObject(this, JsonConverter), Encoding.UTF8);
        }

        public static ABICollectionConfiguration FromJson(string jsonFile)
        {
            var content = File.ReadAllText(jsonFile, Encoding.UTF8);
            var configuration = JsonConvert.DeserializeObject<ABICollectionConfiguration>(content, JsonConverter);
            return configuration;
        }
    }
}
