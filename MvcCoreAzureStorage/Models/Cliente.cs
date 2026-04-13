using Azure;
using Azure.Data.Tables;

namespace MvcCoreAzureStorage.Models
{
    public class Cliente : ITableEntity
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Salario { get; set; }

        //ID CLIENTE: ROWKEY
        //CUANDO EL USUARIO ALMACENE UN ID DE CLIENTE,
        //NOSOTROS ALMACENAMOS ROWKEY
        private int _IdCliente;
        public int IdCliente 
        { 
            get { return this._IdCliente; } 
            set { 
                this._IdCliente = value;
                this.RowKey = value.ToString();
            }
        }

        //EMPRESA: PARTITIONKEY
        //CUANDO EL USUARIO ALMACENE UNA EMPRESA,
        //NOSOTROS ALMACENAMOS PARTITIONKEY
        private string _Empresa;
        public string Empresa 
        { 
            get { return this._Empresa; } 
            set { 
                this._Empresa = value;
                this.PartitionKey = value;
            }
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
