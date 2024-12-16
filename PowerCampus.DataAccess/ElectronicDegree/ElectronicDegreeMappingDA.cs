// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeMappingDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.DataAccess.DataAccess;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// Operations related to Electronic Degree Mapping
    /// </summary>
    public static class ElectronicDegreeMappingDA
    {
        /// <summary>
        /// Deletes the electronic degree mapping.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="electronicDegreeTable">The electronic degree table.</param>
        public static void DeleteElectronicDegreeMapping(string value, string electronicDegreeTable)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<ElectronicDegreeMapping> query = from edm in context.ElectronicDegreeMappings
                                                            where edm.ElectronicDegreeTable == electronicDegreeTable
                                                            && edm.ElectronicDegreeValue == value
                                                            select edm;

                context.ElectronicDegreeMappings.DeleteAllOnSubmit(query);

                context.SubmitChanges();
            }
        }

        /// <summary>
        /// Deletes the electronic degree mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public static void DeleteElectronicDegreeMapping(int id)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                ElectronicDegreeMapping edm = context.ElectronicDegreeMappings.FirstOrDefault(e => e.ElectronicDegreeMappingId == id);
                context.ElectronicDegreeMappings.DeleteOnSubmit(edm);
                context.SubmitChanges();
            }
        }

        /// <summary>
        /// Gets the electronic degree mapping.
        /// </summary>
        /// <param name="electronicDegreeTable">The electronic degree table.</param>
        /// <returns></returns>
        public static IQueryable<ElectronicDegreeMapping> GetElectronicDegreeMapping(string electronicDegreeTable)
        {
            ElectronicDegreeContext context = new ElectronicDegreeContext();
            IQueryable<ElectronicDegreeMapping> query = (from edm in context.ElectronicDegreeMappings
                                                         where edm.ElectronicDegreeTable == electronicDegreeTable
                                                         select edm);
            return query;
        }

        /// <summary>
        /// Saves the electronic degree mapping.
        /// </summary>
        /// <param name="edm">The edm.</param>
        /// <returns></returns>
        public static int SaveElectronicDegreeMapping(ElectronicDegreeMapping edm)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                context.ElectronicDegreeMappings.InsertOnSubmit(edm);
                context.SubmitChanges();
                return edm.ElectronicDegreeMappingId;
            }
        }
    }
}