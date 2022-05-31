using System;
using System.Data;

namespace Motorsazan.CustomerAppWeb.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StoredProcedureParameterAttribute : Attribute
    {
        public string XmlSchemaCollectionDatabase { get; set; }

        public string XmlSchemaCollectionOwningSchema { get; set; }

        public string XmlSchemaCollectionName { get; set; }

        public string ParameterName { get; set; }

        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public object Value { get; set; }

        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        public int Size { get; set; }

        public string SourceColumn { get; set; }

        public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Current;

        public bool SourceColumnNullMapping { get; set; }

        public bool IsNullable { get; set; }

        public SqlDbType SqlDbType { get; set; } = SqlDbType.NVarChar;
    }
}