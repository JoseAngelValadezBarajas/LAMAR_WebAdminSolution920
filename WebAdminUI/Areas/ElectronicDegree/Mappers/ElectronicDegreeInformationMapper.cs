// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeInformationMapper.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicDegree.Models;

namespace WebAdminUI.Areas.ElectronicDegree.Mappers
{
    internal static class ElectronicDegreeInformationMapper
    {
        internal static List<ElectronicDegreeModel> ToViewModel(this List<ElectronicDegreeInfo> electronicDegreeInfoList)
        {
            List<ElectronicDegreeModel> electronicDegreeModelList = new List<ElectronicDegreeModel>();
            foreach (ElectronicDegreeInfo electronicDegreeInfo in electronicDegreeInfoList)
            {
                ElectronicDegreeModel electronicDegreeModel = new ElectronicDegreeModel();
                electronicDegreeModel.DisplayName = electronicDegreeInfo.Student.DisplayName;
                electronicDegreeModel.PeopleCodeId = electronicDegreeInfo.Student.PeopleCodeId;
                electronicDegreeModel.Folio = electronicDegreeInfo.Folio;
                electronicDegreeModel.Major = electronicDegreeInfo.Major;
                electronicDegreeModel.MajorCode = electronicDegreeInfo.MajorCode;
                electronicDegreeModel.EducationLevel = electronicDegreeInfo.EducationLevel;
                electronicDegreeModel.ElectronicDegreeInformationId = electronicDegreeInfo.ElectronicDegreeInformationId;
                electronicDegreeModel.Status = electronicDegreeInfo.Status;
                electronicDegreeModel.HasXML = !string.IsNullOrEmpty(electronicDegreeInfo.RequestXML);
                electronicDegreeModelList.Add(electronicDegreeModel);
            }

            return electronicDegreeModelList;
        }
    }
}