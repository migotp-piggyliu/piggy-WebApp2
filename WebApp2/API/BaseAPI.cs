using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

namespace WebApp2
{
    public class BaseAPI
    {
        protected AppDBContext db;

        public BaseAPI(DbContext db)
        {
            this.db = (AppDBContext)db;
        }

        private int  last_error_code = 0;
        public int LastErrorCode
        {
            get { return last_error_code; }
            set { last_error_code = value;  }
        }

        private string last_error = "";
        public string LastError
        {
            get { return last_error;  }
            set { last_error = value; }
        }

        public void SetLastError(int code, string error)
        {
            last_error_code = code;
            last_error = error;
        }

 
    }
}
