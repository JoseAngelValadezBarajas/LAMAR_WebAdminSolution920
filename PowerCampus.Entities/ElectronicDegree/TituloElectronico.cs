// --------------------------------------------------------------------
// <copyright file="TituloElectronico.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
[XmlRoot(Namespace = "https://www.siged.sep.gob.mx/titulos/", IsNullable = false)]
public partial class TituloElectronico
{
    private string versionField;

    private string folioControlField;

    private TituloElectronicoAntecedente antecedenteField;

    private XmlElement autenticacionField;

    private TituloElectronicoCarrera carreraField;

    private TituloElectronicoExpedicion expedicionField;

    private TituloElectronicoFirmaResponsable[] firmaResponsablesField;
    
    private TituloElectronicoInstitucion institucionField;

    private TituloElectronicoProfesionista profesionistaField;

    

    public TituloElectronico() => versionField = "1.0";

    [XmlElement(Order = 6)]
    public TituloElectronicoAntecedente Antecedente { get => antecedenteField; set => antecedenteField = value; }

    //public XmlElement Autenticacion { get => autenticacionField; set => autenticacionField = value; }

    [XmlElement(Order = 3)]
    public TituloElectronicoCarrera Carrera { get => carreraField; set => carreraField = value; }

    [XmlElement(Order = 5)]
    public TituloElectronicoExpedicion Expedicion { get => expedicionField; set => expedicionField = value; }

    [XmlArray(Order = 1)]
    [XmlArrayItem("FirmaResponsable", IsNullable = false)]
    public TituloElectronicoFirmaResponsable[] FirmaResponsables
    {
        get => firmaResponsablesField;
        set => firmaResponsablesField = value;
    }



    [XmlElement(Order = 2)]
    public TituloElectronicoInstitucion Institucion { get => institucionField; set => institucionField = value; }

    [XmlElement(Order = 4)]
    public TituloElectronicoProfesionista Profesionista
    {
        get => profesionistaField;
        set => profesionistaField = value;
    }

    [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
    public string schemaLocation { get; set; }

    [XmlAttribute()]
    public string version { get => versionField; set => versionField = value; }

    [XmlAttribute()]
    public string folioControl { get => folioControlField; set => folioControlField = value; }
}

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
public partial class TituloElectronicoAntecedente
{
    private string institucionProcedenciaField;

    private string idTipoEstudioAntecedenteField;

    private string tipoEstudioAntecedenteField;

    private string idEntidadFederativaField;

    private string entidadFederativaField;

    private string fechaInicioField;

    private string fechaTerminacionField;

    private string noCedulaField;

    [XmlAttribute()]
    public string institucionProcedencia
    {
        get => institucionProcedenciaField;
        set => institucionProcedenciaField = value;
    }
    [XmlAttribute(DataType = "integer")]
    public string idTipoEstudioAntecedente
    {
        get => idTipoEstudioAntecedenteField;
        set => idTipoEstudioAntecedenteField = value;
    }

    [XmlAttribute()]
    public string tipoEstudioAntecedente
    {
        get => tipoEstudioAntecedenteField;
        set => tipoEstudioAntecedenteField = value;
    }

    [XmlAttribute()]
    public string idEntidadFederativa { get => idEntidadFederativaField; set => idEntidadFederativaField = value; }

    [XmlAttribute()]
    public string entidadFederativa { get => entidadFederativaField; set => entidadFederativaField = value; }

    [XmlAttribute()]
    public string fechaInicio { get => fechaInicioField; set => fechaInicioField = value; }

    [XmlAttribute()]
    public string fechaTerminacion { get => fechaTerminacionField; set => fechaTerminacionField = value; }

    [XmlAttribute()]
    public string noCedula { get => noCedulaField; set => noCedulaField = value; }

}

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
public partial class TituloElectronicoCarrera
{

    private string cveCarreraField;

    private string nombreCarreraField;

    private string fechaInicioField;

    private string fechaTerminacionField;

    private string idAutorizacionReconocimientoField;

    private string autorizacionReconocimientoField;

    private string numeroRvoeField;

    

    [XmlAttribute()]
    public string cveCarrera { get => cveCarreraField; set => cveCarreraField = value; }

    [XmlAttribute()]
    public string nombreCarrera { get => nombreCarreraField; set => nombreCarreraField = value; }

    [XmlAttribute()]
    public string fechaInicio { get => fechaInicioField; set => fechaInicioField = value; }

    [XmlAttribute()]
    public string fechaTerminacion { get => fechaTerminacionField; set => fechaTerminacionField = value; }

    [XmlAttribute(DataType = "integer")]
    public string idAutorizacionReconocimiento
    {
        get => idAutorizacionReconocimientoField;
        set => idAutorizacionReconocimientoField = value;
    }

    [XmlAttribute()]
    public string autorizacionReconocimiento
    {
        get => autorizacionReconocimientoField;
        set => autorizacionReconocimientoField = value;
    }

    [XmlAttribute()]
    public string numeroRvoe { get => numeroRvoeField; set => numeroRvoeField = value; }
}

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
public partial class TituloElectronicoExpedicion
{
    private string fechaExpedicionField;

    private string idModalidadTitulacionField;

    private string modalidadTitulacionField;

    private string fechaExamenProfesionalField;

    private string fechaExencionExamenProfesionalField;

    private string cumplioServicioSocialField;

    private string idFundamentoLegalServicioSocialField;

    private string fundamentoLegalServicioSocialField;

    private string idEntidadFederativaField;

    private string entidadFederativaField;

    [XmlAttribute()]
    public string fechaExpedicion { get => fechaExpedicionField; set => fechaExpedicionField = value; }

    [XmlAttribute(DataType = "integer")]
    public string idModalidadTitulacion
    {
        get => idModalidadTitulacionField;
        set => idModalidadTitulacionField = value;
    }

    [XmlAttribute()]
    public string modalidadTitulacion { get => modalidadTitulacionField; set => modalidadTitulacionField = value; }

 

    [XmlAttribute()]
    public string fechaExamenProfesional
    {
        get => fechaExamenProfesionalField;
        set => fechaExamenProfesionalField = value;
    }

    [XmlAttribute()]
    public string fechaExencionExamenProfesional
    {
        get => fechaExencionExamenProfesionalField;
        set => fechaExencionExamenProfesionalField = value;
    }

    [XmlAttribute(DataType = "integer")]
    public string cumplioServicioSocial
    {
        get => cumplioServicioSocialField;
        set => cumplioServicioSocialField = value;
    }

    [XmlAttribute(DataType = "integer")]
    public string idFundamentoLegalServicioSocial
    {
        get => idFundamentoLegalServicioSocialField;
        set => idFundamentoLegalServicioSocialField = value;
    }

    [XmlAttribute()]
    public string fundamentoLegalServicioSocial
    {
        get => fundamentoLegalServicioSocialField;
        set => fundamentoLegalServicioSocialField = value;
    }

    [XmlAttribute()]
    public string idEntidadFederativa { get => idEntidadFederativaField; set => idEntidadFederativaField = value; }

    
    [XmlAttribute()]
    public string entidadFederativa { get => entidadFederativaField; set => entidadFederativaField = value; }


}

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
public partial class TituloElectronicoFirmaResponsable
{
    private string nombreField;

    private string primerApellidoField;

    private string segundoApellidoField;

    private string curpField;

    private string idCargoField;

    private string cargoField;

    private string abrTituloField;

    private string selloField;

    private string certificadoResponsableField;

    private string noCertificadoResponsableField;

    private string thumbprintField;

    [XmlAttribute()]
    public string nombre { get => nombreField; set => nombreField = value; }

    [XmlAttribute()]
    public string primerApellido { get => primerApellidoField; set => primerApellidoField = value; }

    [XmlAttribute()]
    public string segundoApellido { get => segundoApellidoField; set => segundoApellidoField = value; }

    [XmlAttribute()]
    public string curp { get => curpField; set => curpField = value; }

    [XmlAttribute(DataType = "integer")]
    public string idCargo { get => idCargoField; set => idCargoField = value; }

    [XmlAttribute()]
    public string cargo { get => cargoField; set => cargoField = value; }

    [XmlAttribute()]
    public string abrTitulo { get => abrTituloField; set => abrTituloField = value; }

    [XmlAttribute()]
    public string sello { get => selloField; set => selloField = value; }

    [XmlAttribute()]
    public string certificadoResponsable
    {
        get => certificadoResponsableField;
        set => certificadoResponsableField = value;
    }


    [XmlAttribute()]
    public string noCertificadoResponsable
    {
        get => noCertificadoResponsableField;
        set => noCertificadoResponsableField = value;
    }
    [XmlIgnore]
    public string thumbprint { get => thumbprintField; set => thumbprintField = value; }
}

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
public partial class TituloElectronicoInstitucion
{
    private string cveInstitucionField;

    private string nombreInstitucionField;

    [XmlAttribute()]
    public string cveInstitucion { get => cveInstitucionField; set => cveInstitucionField = value; }

    [XmlAttribute()]
    public string nombreInstitucion { get => nombreInstitucionField; set => nombreInstitucionField = value; }
}

[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/titulos/")]
public partial class TituloElectronicoProfesionista
{
    
    private string curpField;

    private string nombreField;

    private string primerApellidoField;

    private string segundoApellidoField;

    private string correoElectronicoField;

   
    [XmlAttribute()]
    public string curp { get => curpField; set => curpField = value; }

    [XmlAttribute()]
    public string nombre { get => nombreField; set => nombreField = value; }

    [XmlAttribute()]
    public string primerApellido { get => primerApellidoField; set => primerApellidoField = value; }

    [XmlAttribute()]
    public string segundoApellido { get => segundoApellidoField; set => segundoApellidoField = value; }

    [XmlAttribute()]
    public string correoElectronico { get => correoElectronicoField; set => correoElectronicoField = value; }

}