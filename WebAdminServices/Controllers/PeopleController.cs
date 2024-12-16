// --------------------------------------------------------------------
// <copyright file="PeopleController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.Entities;
using System.Security.Claims;
using System.Web.Http;
using WebAdminServices.Filters;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Hedtech.PowerCampus.Logger;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// PeopleController Class
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class PeopleController : ApiController
    {
        private readonly PeopleServices _peopleServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleController"/> class.
        /// </summary>
        public PeopleController()
        {
            _peopleServices = new PeopleServices();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/people/get/{id}")]
        public IHttpActionResult Get(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                People peopleResult = _peopleServices.GetTaxpayerInfo(id);
                return Ok(peopleResult);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="CodeId"></param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/people/Charges/list")]
        public IHttpActionResult GetCharges(PeopleOrgCharges peopleCharge)
        {
            if (User.Identity.IsAuthenticated)
            {
                People peopleResult = _peopleServices.GetCharges(peopleCharge.CodeId, peopleCharge.YTS);
                return Ok(peopleResult);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/people/Save/TaxpayerId")]
        public IHttpActionResult SaveTaxpayerId(People people)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claim = User as ClaimsPrincipal;
                string userName = claim.FindFirst("userName").Value;
                people.FiscalRecordsDefault.InvoicePreferredTaxpayerId = _peopleServices.SaveTaxpayerId(people, userName);
                return Ok(people.FiscalRecordsDefault.InvoicePreferredTaxpayerId);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <param name="PeopleCodeId">The people code identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/people/yts/{PeopleCodeId}")]
        public IHttpActionResult YearTermSession(string PeopleCodeId)
        {
            if (User.Identity.IsAuthenticated)
            {
                People peopleResult = _peopleServices.YearTermSession(PeopleCodeId);
                return Ok(peopleResult);
            }
            else
                return Unauthorized();
        }

        [Route("api/people/yts/getgraduateinfo")]
        [HttpGet]
        public IHttpActionResult GetGraduateInfo(string codestudent)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT * FROM [dbo].[ACADEMIC] WHERE people_ID = '{codestudent}' and GRADUATED ='G' and ACADEMIC_SESSION != ''", connection);
                    JArray jsonArray = new JArray();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JObject obj = new JObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                obj[reader.GetName(i)] = reader.GetValue(i).ToString();
                            }
                            jsonArray.Add(obj);
                        }
                    }

                    connection.Close();
                    return Ok(jsonArray);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/people/yts/getfirstacademicinfo")]
        [HttpGet]
        public IHttpActionResult GetFirstAcademicInfo(string codestudent)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT TOP 1 * FROM [dbo].[ACADEMIC] WHERE people_ID = '{codestudent}' and ACADEMIC_SESSION != ''", connection);
                    JArray jsonArray = new JArray();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JObject obj = new JObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                obj[reader.GetName(i)] = reader.GetValue(i).ToString();
                            }
                            jsonArray.Add(obj);
                        }
                    }

                    connection.Close();
                    return Ok(jsonArray);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/people/yts/getacademicterm")]
        [HttpGet]
        public IHttpActionResult GetAcademicTerm(string originalYear, string academicTerm, string peopleID, string academicSession)
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL con las variables de entrada
                    string query = @"
                        -- Verificamos si existen registros para el año original en la tabla ACADEMICCALENDAR
                        IF EXISTS (SELECT 1
                                FROM [dbo].[ACADEMICCALENDAR]
                                WHERE ACADEMIC_TERM = @AcademicTer
                                    AND ACADEMIC_YEAR = @OriginalYear)
                        BEGIN
                            -- Si hay resultados, seleccionamos los registros
                            SELECT *
                            FROM [dbo].[ACADEMICCALENDAR]
                            WHERE ACADEMIC_TERM = @AcademicTer
                            AND ACADEMIC_YEAR = @OriginalYear AND ACADEMIC_SESSION = @AcademicSession;
                        END
                        ELSE
                        BEGIN
                            -- Incrementamos el año en uno
                            DECLARE @NextYear VARCHAR(4) = CAST(CAST(@OriginalYear AS INT) + 1 AS VARCHAR(4));
                            
                            -- Verificamos si existe al menos un registro en la tabla ACADEMIC para el nuevo año
                            IF EXISTS (SELECT TOP 1 ACADEMIC_TERM
                                    FROM [dbo].[ACADEMIC]
                                    WHERE people_ID = @PeopleID
                                        AND ACADEMIC_SESSION != ''
                                        AND ACADEMIC_YEAR = @NextYear)
                            BEGIN
                                -- Asignamos el valor de ACADEMIC_SESSION a la variable @AcademicSess
                                SELECT TOP 1 @AcademicTer = ACADEMIC_TERM
                                FROM [dbo].[ACADEMIC]
                                WHERE people_ID = @PeopleID
                                AND ACADEMIC_SESSION != ''
                                AND ACADEMIC_YEAR = @NextYear;

                                -- Realizamos la consulta en la tabla ACADEMICCALENDAR con el año incrementado
                                SELECT *
                                FROM [dbo].[ACADEMICCALENDAR]
                                WHERE ACADEMIC_TERM = @AcademicTer
                                AND ACADEMIC_YEAR = @NextYear AND ACADEMIC_SESSION = @AcademicSession;
                            END
                            ELSE
                            BEGIN
                                SELECT 'No se encontraron registros para el año siguiente.' AS ErrorMessage;
                            END
                        END";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Añadimos los parámetros a la consulta
                        command.Parameters.AddWithValue("@OriginalYear", originalYear);
                        command.Parameters.AddWithValue("@AcademicTer", academicTerm);
                        command.Parameters.AddWithValue("@PeopleID", peopleID);
                        command.Parameters.AddWithValue("@AcademicSession", academicSession);

                        JArray jsonArray = new JArray();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                JObject obj = new JObject();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    obj[reader.GetName(i)] = reader.GetValue(i).ToString();
                                }
                                jsonArray.Add(obj);
                            }
                        }

                        return Ok(jsonArray);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/people/yts/getcampusrvoe")]
        [HttpGet]
        public IHttpActionResult GetCampusRVOE(string campusRvoe)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Query para obtener la primera y última fila
                    string query = @"
                        WITH OrderedData AS (
                            SELECT ACADEMIC_YEAR, ACADEMIC_TERM, ACADEMIC_SESSION,
                                ROW_NUMBER() OVER (ORDER BY ACADEMIC_YEAR DESC) AS RowNum
                            FROM [Campus_Lamar07sep2023].[dbo].[ACADEMIC]
                            WHERE PEOPLE_ID = @campusRvoe
                            AND ACADEMIC_SESSION != ''
                        ),
                        FirstLastRows AS (
                            SELECT ACADEMIC_YEAR, ACADEMIC_TERM, ACADEMIC_SESSION, RowNum
                            FROM OrderedData
                            WHERE RowNum = 1 OR RowNum = (SELECT MAX(RowNum) FROM OrderedData)
                        )
                        -- Obtener END_DATE para la primera fila
                        SELECT 'FirstRow' AS RowType, FLR.ACADEMIC_YEAR, FLR.ACADEMIC_TERM, FLR.ACADEMIC_SESSION, AC.END_DATE AS Date
                        FROM FirstLastRows FLR
                        JOIN [Campus_Lamar07sep2023].[dbo].[ACADEMICCALENDAR] AC
                        ON FLR.ACADEMIC_YEAR = AC.ACADEMIC_YEAR
                        AND FLR.ACADEMIC_TERM = AC.ACADEMIC_TERM
                        AND FLR.ACADEMIC_SESSION = AC.ACADEMIC_SESSION
                        WHERE FLR.RowNum = 1

                        UNION ALL

                        -- Obtener START_DATE para la última fila
                        SELECT 'LastRow' AS RowType, FLR.ACADEMIC_YEAR, FLR.ACADEMIC_TERM, FLR.ACADEMIC_SESSION, AC.START_DATE AS Date
                        FROM FirstLastRows FLR
                        JOIN [Campus_Lamar07sep2023].[dbo].[ACADEMICCALENDAR] AC
                        ON FLR.ACADEMIC_YEAR = AC.ACADEMIC_YEAR
                        AND FLR.ACADEMIC_TERM = AC.ACADEMIC_TERM
                        AND FLR.ACADEMIC_SESSION = AC.ACADEMIC_SESSION
                        WHERE FLR.RowNum = (SELECT MAX(RowNum) FROM FirstLastRows);";
                    
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@codestudent", campusRvoe);
                    
                    JArray jsonArray = new JArray();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", reader["RowType"].ToString());
                            JObject obj = new JObject
                            {
                                ["RowType"] = reader["RowType"].ToString(),
                                ["ACADEMIC_YEAR"] = reader["ACADEMIC_YEAR"].ToString(),
                                ["ACADEMIC_TERM"] = reader["ACADEMIC_TERM"].ToString(),
                                ["ACADEMIC_SESSION"] = reader["ACADEMIC_SESSION"].ToString(),
                                ["Date"] = reader["Date"].ToString()
                            };
                            jsonArray.Add(obj);
                        }
                    }

                    connection.Close();
                    return Ok(jsonArray);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/people/yts/getgraduatedates")]
        [HttpGet]
        public IHttpActionResult GetGraduateDates(string codestudent, string institutionName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        DECLARE @curriculumValue VARCHAR(50);
                        DECLARE @startDate DATE;
                        DECLARE @endDate DATE;

                        -- Obtener el código del currículo basado en el nombre de la institución
                        SELECT @curriculumValue = CODE_VALUE
                        FROM [CODE_CURRICULUM]
                        WHERE FORMAL_TITLE = @institutionName;

                        -- Verificar si se encontró el currículo
                        IF @curriculumValue IS NULL
                        BEGIN
                            RAISERROR('No se encontró el currículo para la institución especificada', 16, 1);
                            RETURN;
                        END;

                        ;WITH OrderedData AS (
                            SELECT ACADEMIC_YEAR, ACADEMIC_TERM, ACADEMIC_SESSION,
                                ROW_NUMBER() OVER (ORDER BY ACADEMIC_YEAR DESC) AS RowNum
                            FROM [ACADEMIC]
                            WHERE PEOPLE_ID = @codestudent
                            AND ACADEMIC_SESSION != ''
                            AND CURRICULUM = @curriculumValue
                        ),
                        FirstLastRows AS (
                            SELECT ACADEMIC_YEAR, ACADEMIC_TERM, ACADEMIC_SESSION, RowNum
                            FROM OrderedData
                            WHERE RowNum = 1 OR RowNum = (SELECT MAX(RowNum) FROM OrderedData)
                        ),
                        Dates AS (
                            -- Obtener END_DATE para la primera fila
                            SELECT 'FirstRow' AS RowType, FLR.ACADEMIC_YEAR, FLR.ACADEMIC_TERM, FLR.ACADEMIC_SESSION, AC.END_DATE AS Date
                            FROM FirstLastRows FLR
                            JOIN [ACADEMICCALENDAR] AC
                            ON FLR.ACADEMIC_YEAR = AC.ACADEMIC_YEAR
                            AND FLR.ACADEMIC_TERM = AC.ACADEMIC_TERM
                            AND FLR.ACADEMIC_SESSION = AC.ACADEMIC_SESSION
                            WHERE FLR.RowNum = 1

                            UNION ALL

                            -- Obtener START_DATE para la última fila
                            SELECT 'LastRow' AS RowType, FLR.ACADEMIC_YEAR, FLR.ACADEMIC_TERM, FLR.ACADEMIC_SESSION, AC.START_DATE AS Date
                            FROM FirstLastRows FLR
                            JOIN [ACADEMICCALENDAR] AC
                            ON FLR.ACADEMIC_YEAR = AC.ACADEMIC_YEAR
                            AND FLR.ACADEMIC_TERM = AC.ACADEMIC_TERM
                            AND FLR.ACADEMIC_SESSION = AC.ACADEMIC_SESSION
                            WHERE FLR.RowNum = (SELECT MAX(RowNum) FROM FirstLastRows)
                        )

                        -- Asignar las fechas a las variables
                        SELECT 
                            @startDate = MAX(CASE WHEN RowType = 'LastRow' THEN Date END),
                            @endDate = MAX(CASE WHEN RowType = 'FirstRow' THEN Date END)
                        FROM Dates;

                        -- Seleccionar los resultados finales para devolver como JSON
                        SELECT 'FirstRow' AS RowType, FLR.ACADEMIC_YEAR, FLR.ACADEMIC_TERM, FLR.ACADEMIC_SESSION, AC.END_DATE AS Date
                        FROM FirstLastRows FLR
                        JOIN [ACADEMICCALENDAR] AC
                        ON FLR.ACADEMIC_YEAR = AC.ACADEMIC_YEAR
                        AND FLR.ACADEMIC_TERM = AC.ACADEMIC_TERM
                        AND FLR.ACADEMIC_SESSION = AC.ACADEMIC_SESSION
                        WHERE FLR.RowNum = 1

                        UNION ALL

                        SELECT 'LastRow' AS RowType, FLR.ACADEMIC_YEAR, FLR.ACADEMIC_TERM, FLR.ACADEMIC_SESSION, AC.START_DATE AS Date
                        FROM FirstLastRows FLR
                        JOIN [ACADEMICCALENDAR] AC
                        ON FLR.ACADEMIC_YEAR = AC.ACADEMIC_YEAR
                        AND FLR.ACADEMIC_TERM = AC.ACADEMIC_TERM
                        AND FLR.ACADEMIC_SESSION = AC.ACADEMIC_SESSION
                        WHERE FLR.RowNum = (SELECT MAX(RowNum) FROM FirstLastRows);";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@codestudent", codestudent);
                    command.Parameters.AddWithValue("@institutionName", institutionName);

                    JArray jsonArray = new JArray();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JObject obj = new JObject
                            {
                                ["RowType"] = reader["RowType"].ToString(),
                                ["ACADEMIC_YEAR"] = reader["ACADEMIC_YEAR"].ToString(),
                                ["ACADEMIC_TERM"] = reader["ACADEMIC_TERM"].ToString(),
                                ["ACADEMIC_SESSION"] = reader["ACADEMIC_SESSION"].ToString(),
                                ["Date"] = reader["Date"].ToString()
                            };
                            jsonArray.Add(obj);
                        }
                    }

                    connection.Close();
                    return Ok(jsonArray);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}