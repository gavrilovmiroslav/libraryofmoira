using System.Linq;

using Notion.Client;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfMoira.DataManagement
{
    public struct NotionQuery 
    {
        public string Table;
        public DatabasesQueryParameters Filters;
    }

    public class NotionDataPlugin 
        : DataImport<PaginatedList<Page>, NotionQuery>        
    {
        private static readonly string NotionAuthToken = "secret_UbfdBhWj3OXxzzgWNDuZW97HK4FFsIUBuHsamwTYj4T";
        private static readonly string NotionMetaDB = "db52fee52e8c41048ab7a7454e42cea5";
        private NotionClient _client;
        private Dictionary<string, string> _meta;

        public NotionDataPlugin()
        {
            _client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = NotionAuthToken
            });

            _meta = new Dictionary<string, string>();
            var metaTask = Task.Run(async () => await _client.Databases.QueryAsync(NotionMetaDB, new DatabasesQueryParameters()));
            metaTask.Wait();

            if (metaTask.IsCompletedSuccessfully)
            {
                var meta = metaTask.Result;
                foreach (var prop in meta.Results)
                {
                    var name = (prop.Properties["Name"] as TitlePropertyValue).Title[0].PlainText;
                    var id = ((prop.Properties["Database Id"] as RollupPropertyValue).Rollup.Array[0] as RichTextPropertyValue).RichText[0].PlainText;

                    _meta[name] = id;
                }  
            }
            else
            {
                throw new Exception("Meta DB failed to load, quitting!");
            }
        }

        public async Task<PaginatedList<Page>> GetAsync(string tableName)
        {
            return await GetAsync(new NotionQuery { Table = tableName, Filters = new DatabasesQueryParameters() });
        }

        public async Task<PaginatedList<Page>> GetAsync(NotionQuery scope)
        {
            if (_meta.ContainsKey(scope.Table))
            {
                return await _client.Databases.QueryAsync(_meta[scope.Table], scope.Filters);
            }
            else
            {
                return null;
            }
        }

        public List<T> TranslateInto<T>(PaginatedList<Page> data) where T : new()
        {
            var result = new List<T>();
            if (data.Results.Count == 0) return result;

            var fields = new HashSet<string>();
            var aliases = new Dictionary<string, string>();
            foreach (var field in typeof(T).GetFields())
            {
                fields.Add(field.Name);
                var attr = field.GetCustomAttribute<QueryAsAttribute>();
                if (attr != null)
                {
                    aliases.Add(attr.Name, field.Name);
                }
            }

            var propFields = new Dictionary<string, FieldInfo>();
            foreach (var prop in data.Results[0].Properties.Keys)
            {
                if (fields.Contains(prop))
                {
                    propFields[prop] = typeof(T).GetField(prop);
                }
                else if (aliases.ContainsKey(prop))
                {
                    propFields[prop] = typeof(T).GetField(aliases[prop]);
                }
                else
                {
                    throw new Exception($"Data source needs to have a field called '{prop}'.");
                }
            }

            foreach (var row in data.Results)
            {
                var instance = new T();
                foreach (var propField in propFields)
                {
                    var prop = row.Properties[propField.Key];
                    if (prop is RichTextPropertyValue rtv)
                    {
                        var str = string.Join("", rtv.RichText.Select(r => r.PlainText));                        
                        if (propField.Value.FieldType == typeof(string))
                        {
                            propField.Value.SetValue(instance, str);
                        }
                        else if (propField.Value.FieldType == typeof(int))
                        {
                            propField.Value.SetValue(instance, int.Parse(str));
                        }
                        else if (propField.Value.FieldType == typeof(float))
                        {
                            propField.Value.SetValue(instance, float.Parse(str));
                        }
                        else if (propField.Value.FieldType == typeof(bool))
                        {
                            var s = str.ToLowerInvariant();
                            propField.Value.SetValue(instance, s == "yes" || s == "true");
                        }
                    }
                    else if (prop is NumberPropertyValue numv)
                    {
                        if (propField.Value.FieldType == typeof(string))
                        {
                            propField.Value.SetValue(instance, $"{numv.Number}");
                        }
                        else if (propField.Value.FieldType == typeof(int))
                        {
                            propField.Value.SetValue(instance, (int)numv.Number);
                        }
                        else if (propField.Value.FieldType == typeof(float))
                        {
                            propField.Value.SetValue(instance, (float)numv.Number);
                        }
                        else if (propField.Value.FieldType == typeof(bool))
                        {
                            propField.Value.SetValue(instance, numv.Number > 0);
                        }
                    }
                    else if (prop is CheckboxPropertyValue chv)
                    {
                        if (propField.Value.FieldType == typeof(string))
                        {
                            propField.Value.SetValue(instance, chv.Checkbox ? "yes" : "no");
                        }
                        else if (propField.Value.FieldType == typeof(int))
                        {
                            propField.Value.SetValue(instance, chv.Checkbox ? 1 : 0);
                        }
                        else if (propField.Value.FieldType == typeof(float))
                        {
                            propField.Value.SetValue(instance, chv.Checkbox ? 1.0f : 0.0f);
                        }
                        else if (propField.Value.FieldType == typeof(bool))
                        {
                            propField.Value.SetValue(instance, chv.Checkbox);
                        }
                    }
                    else if (prop is RollupPropertyValue rollv)
                    {
                        if (propField.Value.FieldType == typeof(string))
                        {
                            var str = string.Join("", (rollv.Rollup.Array[0] as RichTextPropertyValue).RichText.Select(r => r.PlainText));
                            propField.Value.SetValue(instance, str);
                        }
                        else if (propField.Value.FieldType == typeof(int))
                        {
                            propField.Value.SetValue(instance, (int)rollv.Rollup.Number);
                        }
                        else if (propField.Value.FieldType == typeof(float))
                        {
                            propField.Value.SetValue(instance, (float)rollv.Rollup.Number);
                        }
                        else if (propField.Value.FieldType == typeof(bool))
                        {
                            propField.Value.SetValue(instance, rollv.Rollup.Number > 0);
                        }
                    }                    
                    else if (prop is TitlePropertyValue ttv)
                    {
                        var str = string.Join("", ttv.Title.Select(r => r.PlainText));
                        if (propField.Value.FieldType == typeof(string))
                        {
                            propField.Value.SetValue(instance, str);
                        }
                        else if (propField.Value.FieldType == typeof(int))
                        {
                            propField.Value.SetValue(instance, int.Parse(str));
                        }
                        else if (propField.Value.FieldType == typeof(float))
                        {
                            propField.Value.SetValue(instance, float.Parse(str));
                        }
                        else if (propField.Value.FieldType == typeof(bool))
                        {
                            var s = str.ToLowerInvariant();
                            propField.Value.SetValue(instance, s == "yes" || s == "true");
                        }
                    }
                    else if (prop is MultiSelectPropertyValue msv)
                    {
                        if (propField.Value.FieldType == typeof(HashSet<string>))
                        {
                            HashSet<string> set = new HashSet<string>();
                            foreach (var opt in msv.MultiSelect)
                            {
                                set.Add(opt.Name);
                            }
                            propField.Value.SetValue(instance, set);
                        } 
                        else if (propField.Value.FieldType == typeof(HashSet<int>))
                        {
                            HashSet<int> set = new HashSet<int>();
                            foreach (var opt in msv.MultiSelect)
                            {
                                set.Add(int.Parse(opt.Name));
                            }
                            propField.Value.SetValue(instance, set);
                        }
                        else if (propField.Value.FieldType == typeof(HashSet<float>))
                        {
                            HashSet<float> set = new HashSet<float>();
                            foreach (var opt in msv.MultiSelect)
                            {
                                set.Add(float.Parse(opt.Name));
                            }
                            propField.Value.SetValue(instance, set);
                        }
                    }
                }
                result.Add(instance);
            }

            return result;
        }

        public async Task<List<T>> GetAsAsync<T>(NotionQuery scope) where T : new()
        {
            var raw = await GetAsync(scope);
            return TranslateInto<T>(raw);
        }

        public async Task<List<T>> GetAsAsync<T>(string tableName) where T : new()
        {
            return await GetAsAsync<T>(new NotionQuery { Table = tableName, Filters = new DatabasesQueryParameters() });
        }
    }
}
