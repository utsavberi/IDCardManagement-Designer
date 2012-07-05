using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCardManagement
{
    public class IDCard
    {
        public String dataSourceType, primaryKey;
        public String connectionString;
        public String tableName;
        public System.Drawing.Size dimensions;
        public System.Drawing.Image backgroundImage;
        public ArrayList fields;
        public ArrayList selectedFields;



        public IDCard(string connectionString, string dataSourceType, string tableName,string primaryKey, System.Drawing.Size dimensions, System.Drawing.Image backgroundImage, ArrayList fields, ArrayList selectedFields,String title)
        {
            this.primaryKey = primaryKey;
            this.dataSourceType = dataSourceType;
            this.title = title;
            this.connectionString = connectionString;
            this.tableName = tableName;
            this.dimensions = dimensions;
            this.backgroundImage = backgroundImage;
            this.fields = fields;
            this.selectedFields = selectedFields;
        }

        public override string ToString()
        {
            return connectionString +"  " +tableName+"  " +dimensions+"  " +backgroundImage+"  " +fields+"  " +selectedFields;
        }


        public string title { get; set; }
    }
}
