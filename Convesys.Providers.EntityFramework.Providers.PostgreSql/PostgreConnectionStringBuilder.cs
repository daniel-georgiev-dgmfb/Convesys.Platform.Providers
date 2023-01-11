//  ----------------------------------------------------------------------- 
//   <copyright file="SQLServerConnectionStringBuilder.cs" company="Glasswall Solutions Ltd.">
//       Glasswall Solutions Ltd.
//   </copyright>
//  ----------------------------------------------------------------------- 

namespace Pirina.Providers.EntityFramework.Providers.PostgreSql.Connection
{
    using Kernel.Data.Connection;
    using Kernel.Initialisation;
    using Npgsql;
    using Pirina.Kernel.Configuration;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     Build a SQLServer connection string from an IDbConnectionDefinition
    /// </summary>
    public class PostgreConnectionStringBuilder : IConnectionStringProvider<NpgsqlConnectionStringBuilder>,
        IAutoRegisterAsTransient
    {
        #region fields

        private readonly IDbConnectionDefinition _definition;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an instance of SQLServerConnectionStringBuilder
        /// </summary>
        /// <param name="parser">Parses nameValue colection to database connection definition</param>
        public PostgreConnectionStringBuilder(IConnectionDefinitionParser parser)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");

            _definition = parser.ConnectionDefinition;
        }

        public PostgreConnectionStringBuilder(IConfiguration configuration, IDbConnectionDefinition dbConnectionDefinition = null)
        {
            this._configuration = configuration;
            this._definition = dbConnectionDefinition;
        }

        /// <summary>
        ///     Creates an instance of SQLServerConnectionStringBuilder
        /// </summary>
        /// <param name="definition">Databse connection definition</param>
        protected PostgreConnectionStringBuilder(IDbConnectionDefinition definition)
        {
            _definition = definition;
        }

        #endregion



        #region Methods

        /// <summary>
        ///     Validate and the get connection string
        /// </summary>
        /// <returns>The connection string</returns>
        public NpgsqlConnectionStringBuilder GetConnectionString()
        {
            try
            {
                if (this._definition != null)
                    return this.BuildFromDefinition();
                var connectionString = this._configuration.GetValue<string>("PsgrConnectionString");
                var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
                return builder;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        private NpgsqlConnectionStringBuilder BuildFromDefinition()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region static methods

        /// <summary>
        ///     Builds from definiton.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns></returns>
        public static PostgreConnectionStringBuilder BuildFromDefiniton(IDbConnectionDefinition definition)
        {
            return new PostgreConnectionStringBuilder(definition);
        }

        /// <summary>
        ///     Validates the Connection Definition.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="validationResult">The validation result.</param>
        /// <returns><c>true</c> if required fields are provided., <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool ValidateDefinition(IDbConnectionDefinition definition,
            IList<ValidationResult> validationResult)
        {
            var members = new List<string>();

            if (string.IsNullOrWhiteSpace(definition.DataSource))
                members.Add("DataSource");

            if (string.IsNullOrWhiteSpace(definition.Database))
                members.Add("Database");

            if (!definition.IntegratedSecurity)
            {
                if (string.IsNullOrWhiteSpace(definition.UserName))
                    members.Add("UserName");

                if (string.IsNullOrWhiteSpace(definition.Password))
                    members.Add("Password");
            }

            if (members.Count == 0)
                return true;

            validationResult.Add(new ValidationResult("DbConnectioDefinition is invalid.", members));

            return false;
        }

        private static string AggregateValidationErrorMessage(IEnumerable<ValidationResult> validationResult)
        {
            return validationResult.Aggregate
            (
                new StringBuilder(),
                (stringBuilder, item) =>
                {
                    stringBuilder.AppendLine(item.ErrorMessage);
                    if (item.MemberNames != null)
                    {
                        stringBuilder.AppendLine("Missing members:");
                        stringBuilder.Append(string.Join(", ", item.MemberNames));
                    }

                    return stringBuilder;
                },
                result => result.ToString()
            );
        }

        #endregion
    }
}