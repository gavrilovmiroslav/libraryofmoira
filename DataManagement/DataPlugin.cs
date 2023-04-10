using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LibraryOfMoira.DataManagement
{
    public class QueryAsAttribute : Attribute
    {
        public string Name;

        public QueryAsAttribute(string name)
        {
            Name = name;
        }
    }

    public interface DataTranslator<Data>
    {
        public List<T> TranslateInto<T>(Data data) where T : new();
    }

    public interface DataQuery<Data, Scope> where Data : class
    {
        public Task<Data> GetAsync(Scope scope);        
    }

    public interface DataImport<Data, Scope> : DataTranslator<Data>, DataQuery<Data, Scope>
        where Data : class
    {
        public Task<List<T>> GetAsAsync<T>(Scope scope) where T : new();
    }
 }
