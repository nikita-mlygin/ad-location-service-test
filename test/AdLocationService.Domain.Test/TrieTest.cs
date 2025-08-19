using System;
using System.Collections.Generic;
using System.Linq;
using AdLocationService.Domain.AdLocation;
using Xunit;

namespace AdLocationService.Domain.Test
{
    public class LocationTrieTests
    {
        private static LocationTrieNode<string> BuildTestTrie()
        {
            var root = new LocationTrieNode<string>();

            // вставка всех площадок из примера
            root.Insert("/ru".Split('/', StringSplitOptions.RemoveEmptyEntries), "Яндекс.Директ");
            root.Insert(
                "/ru/svrd".Split('/', StringSplitOptions.RemoveEmptyEntries),
                "Крутая реклама"
            );
            root.Insert(
                "/ru/svrd/revda".Split('/', StringSplitOptions.RemoveEmptyEntries),
                "Ревдинский рабочий"
            );
            root.Insert(
                "/ru/svrd/pervik".Split('/', StringSplitOptions.RemoveEmptyEntries),
                "Ревдинский рабочий"
            );
            root.Insert(
                "/ru/msk".Split('/', StringSplitOptions.RemoveEmptyEntries),
                "Газета уральских москвичей"
            );
            root.Insert(
                "/ru/permobl".Split('/', StringSplitOptions.RemoveEmptyEntries),
                "Газета уральских москвичей"
            );
            root.Insert(
                "/ru/chelobl".Split('/', StringSplitOptions.RemoveEmptyEntries),
                "Газета уральских москвичей"
            );

            return root;
        }

        [Fact]
        public void Test_GlobalLocation()
        {
            var root = BuildTestTrie();
            var result = new HashSet<string>();
            root.Search("/ru".Split('/', StringSplitOptions.RemoveEmptyEntries), result);

            Assert.Single(result);
            Assert.Contains("Яндекс.Директ", result);
        }

        [Fact]
        public void Test_MediumLocation()
        {
            var root = BuildTestTrie();
            var result = new HashSet<string>();
            root.Search("/ru/svrd".Split('/', StringSplitOptions.RemoveEmptyEntries), result);

            Assert.Equal(2, result.Count);
            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Крутая реклама", result);
        }

        [Fact]
        public void Test_DeepLocationRevda()
        {
            var root = BuildTestTrie();
            var result = new HashSet<string>();
            root.Search("/ru/svrd/revda".Split('/', StringSplitOptions.RemoveEmptyEntries), result);

            Assert.Equal(3, result.Count);
            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Крутая реклама", result);
            Assert.Contains("Ревдинский рабочий", result);
        }

        [Fact]
        public void Test_DeepLocationPervik()
        {
            var root = BuildTestTrie();
            var result = new HashSet<string>();
            root.Search(
                "/ru/svrd/pervik".Split('/', StringSplitOptions.RemoveEmptyEntries),
                result
            );

            Assert.Equal(3, result.Count);
            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Крутая реклама", result);
            Assert.Contains("Ревдинский рабочий", result);
        }

        [Fact]
        public void Test_SpecificLocationMsk()
        {
            var root = BuildTestTrie();
            var result = new HashSet<string>();
            root.Search("/ru/msk".Split('/', StringSplitOptions.RemoveEmptyEntries), result);

            Assert.Equal(2, result.Count);
            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Газета уральских москвичей", result);
        }

        [Fact]
        public void Test_UnknownLocation()
        {
            var root = BuildTestTrie();
            var result = new HashSet<string>();
            root.Search("/ru/unknown".Split('/', StringSplitOptions.RemoveEmptyEntries), result);

            // должно вернуть только глобальные площадки от корня
            Assert.Single(result);
            Assert.Contains("Яндекс.Директ", result);
        }

        [Fact]
        public void Test_Traversal()
        {
            var root = BuildTestTrie();
            var allNodes = root.ToList();

            // проверяем, что все ключи присутствуют
            var keys = allNodes.Select(kv => kv.Key).ToHashSet();
            Assert.Contains("/ru", keys);
            Assert.Contains("/ru/svrd", keys);
            Assert.Contains("/ru/svrd/revda", keys);
            Assert.Contains("/ru/svrd/pervik", keys);
            Assert.Contains("/ru/msk", keys);
        }
    }
}
