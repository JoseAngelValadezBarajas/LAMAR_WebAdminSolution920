$(document).ready(function () {
    var createForm = $('#formCreate');

    $('#btnCancel').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        window.localStorage.setItem("CampusCodeId", $("#CampusCodeId").val());
        window.localStorage.setItem("MatricYear", $("#MatricYear").val());
        window.localStorage.setItem("MatricTerm", $("#MatricTerm").val());
        window.location.href = urlAcademicPlanIndex;
    });

    $('[id^=SepId_]').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });

    $('#btnSave').click(function () {
        $("#Processing").css('display', 'block');
        $("#Overlaydiv").css('display', 'block');
        var cols = [];
        var i = 0;
        var indexCourse = $('input#hdnindexCourse').val();
        var icd = new Object();
        var hasEmpty = false;
        for (var i = 0; i < indexCourse; i++) {
            if ($("#EventSubTypeDesc_" + i).val() !== '') {
                var icd = new Object();
                icd.AcademicPlanCourseCatalogId = $('#AcademicPlanId_' + i).val();
                icd.Classification = $('#Classification_' + i).val();
                icd.Credits = $('#Credits_' + i).data('id');
                icd.Discipline = $('#Discipline_' + i).data('id');
                icd.EventId = $('#EventId_' + i).data('id');
                icd.EventSubType = $("#EventSubType_" + i).val();
                icd.RvoeId = $("#RvoeId").val();
                if ($("#SepId_" + i).val() !== '') {
                    hideError('SepId_' + i);
                }
                else {
                    hasEmpty = true;
                    showError('SepId_' + i);
                }
                icd.SepId = $("#SepId_" + i).val();
                icd.SubjectTypeId = $("#SubjectTypeId_" + i).val();
                cols.push(icd);
            }
        };
        if (!hasEmpty) {
            $.ajax({
                url: urlSaveCourses,
                dataType: "json",
                cache: false,
                type: "POST",
                data: { pdcCourses: cols },
                success: function (response) {
                    if (response.id === 1) {
                        $("#divError").css('display', 'none');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                        window.localStorage.setItem("CampusCodeId", $("#CampusCodeId").val());
                        window.localStorage.setItem("MatricYear", $("#MatricYear").val());
                        window.localStorage.setItem("MatricTerm", $("#MatricTerm").val());
                        window.location.href = urlAcademicPlanIndex;
                    }
                    else if (response.id === 0) {
                        window.location.href = urlUnauthorized;
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                    else {
                        $("#divError").css('display', 'block');
                        $("#Processing").css('display', 'none');
                        $("#Overlaydiv").css('display', 'none');
                    }
                }
            });
        }
        else {
            $("#Processing").css('display', 'none');
            $("#Overlaydiv").css('display', 'none');
        }
    });
   
});