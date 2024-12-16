$(document).ready(function () {

    var SignerId = [];
    var InstitutionId = 0;

    $('button').click(function () {
        var id = $(this).attr('id');
        var instId = id.split('_');
        switch (String(instId[0])) {
            case "btnEdit":
                if ($('#divSigner_' + instId[1]).css('display') === 'none') {
                    $('#ullInstSigner_' + instId[1]).css('display', 'none');
                    $('#divSigner_' + instId[1]).css('display', 'block'); 
                }
                else {
                    $('#ullInstSigner_' + instId[1]).css('display', 'block');
                    $('#divSigner_' + instId[1]).css('display', 'none'); 
                }                               
                break;

            case "ddlSignersChk":
                if ($('#ull_' + + instId[1]).css('display') === 'none')
                    $('#ull_' + + instId[1]).css('display', 'block');
                else
                    $('#ull_' + + instId[1]).css('display', 'none');
                break;

            case "btnApply":
                $('input[type=checkbox]:checked').each(function () {
                    var id = $(this).attr('id');
                    var instSignId = id.split('_');
                    if (Number(instSignId[1]) === Number(instId[1])) {
                        InstitutionId = Number(instSignId[1]);
                        SignerId.push(Number(instSignId[2]));
                    }                    
                });

                if (SignerId.length > 0) {
                    $('#ull_' + + instId[1]).css('display', 'none');
                    $('#divSigner_' + instId[1]).css('display', 'none');
                    $("#Processing").css('display', 'block');
                    $("#Overlaydiv").css('display', 'block');                   

                    var Model = {
                        SignerId: SignerId,
                        InstitutionId: InstitutionId,
                        userName: userName
                    };

                    $.ajax({
                        url: urlCreateSignerInstitution,
                        dataType: "json",
                        cache: false,
                        type: "POST",
                        data: { Model: JSON.stringify(Model) },
                        success: function (response) {
                            if (response.id === 1) {
                                $("#divError").css('display', 'none');
                                $("#Processing").css('display', 'none');
                                $("#Overlaydiv").css('display', 'none');
                                window.location.href = urlSignerInstitution;
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
                    SignerId = [];
                }                
                break;
        }        
    });    
});