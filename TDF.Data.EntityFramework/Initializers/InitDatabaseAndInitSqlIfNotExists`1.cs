using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Text;
using System.Transactions;
using TDF.Core.Configuration;
using TDF.Core.Exceptions;

namespace TDF.Data.EntityFramework.Initializers
{
    public class InitDatabaseAndInitSqlIfNotExists<T> : IDatabaseInitializer<T> where T : System.Data.Entity.DbContext
    {
        public void InitializeDatabase(T context)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!context.Database.Exists())
                {
                    throw new KnownException("请先手动创建数据库");
                }
            }
            if (Configs.Instance.DatabaseInitCompleted)
            {
                return;
            }
            Configs.Instance.DatabaseInitCompleted = true;
            CreateDatabase(context);
            context.SaveChanges();
            Seed(context);
        }

        protected virtual void CreateDatabase(T context)
        {
            var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
            context.Database.ExecuteSqlCommand(dbCreationScript);
        }

        public virtual string InitSqlPath { get; set; }

        protected virtual void Seed(T context)
        {
            var commands = ParseCommands(InitSqlPath);
            foreach (var command in commands)
            {
                context.Database.ExecuteSqlCommand(command);
            }
        }

        protected virtual string[] ParseCommands(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new string[0];
            }

            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }
            return statements.ToArray();
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}