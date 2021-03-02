<%@ Page Language="C#" MasterPageFile="~/Default.Master"   AutoEventWireup="true" CodeBehind="Formularios.aspx.cs" Inherits="ALO.WebSite.Formularios.Formularios" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- =================================================================================================================== -->
    <!-- ESTILOS                                                                                                             -->
    <!-- =================================================================================================================== -->
    <style type="text/css">

        .checkbox
        {
            padding-left: 20px;
        }
        .checkbox label
        {
            display: inline-block;
            vertical-align: middle;
            position: relative;
            padding-left: 5px;
        }
        .checkbox label::before
        {
            content: "";
            display: inline-block;
            position: absolute;
            width: 17px;
            height: 17px;
            left: 0;
            margin-left: -20px;
            border: 1px solid #cccccc;
            border-radius: 3px;
            background-color: #fff;
            -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
            -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
            transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
        }
        .checkbox label::after
        {
            display: inline-block;
            position: absolute;
            width: 16px;
            height: 16px;
            left: 0;
            top: 0;
            margin-left: -20px;
            padding-left: 3px;
            padding-top: 1px;
            font-size: 11px;
            color: #555555;
            
        }
        .checkbox input[type="checkbox"]
        {
            opacity: 0;
            z-index: 1;
        }
        .checkbox input[type="checkbox"]:checked + label::after
        {
            font-family: "FontAwesome";
            content: "\f00c";
        }
         
        .checkbox-primary input[type="checkbox"]:checked + label::before
        {
            background-color: #337ab7;
            border-color: #337ab7;
        }
        .checkbox-primary input[type="checkbox"]:checked + label::after
        {
            color: #fff;
        }
        
        
        .CajaTexto
        {
            width :100%;
            height : 24px;
        }
       
        .CajaFecha
        {
            width :89%;
            height : 24px;
        }



    </style>
    <!-- =================================================================================================================== -->
    <!-- FIN ESTILOS                                                                                                         -->
    <!-- =================================================================================================================== -->

    <!-- =================================================================================================================== -->
    <!-- SCRIPT                                                                                                              -->
    <!-- =================================================================================================================== -->
    <script type="text/javascript">

        //======================================================================================
        // LOAD PAGINA                                                                        
        //======================================================================================
        function pageLoad() {


            $(document).ready(function () {


                //==============================================================================
                // DATOS DE TAB 
                //==============================================================================


                $(".selectpicker").selectpicker();

                $(".tablaConBuscador").DataTable({
                    "destroy": true,
                    "stateSave": true,
                    "pageLength": 10,
                    "paging": true,
                    "lengthChange": false,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": false,

                });

                $(".TextFecha").datepicker({
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    showOn: "button",
                    buttonImage: '<%=UrlWeb("~/IMAGES/date.png")%>',
                    buttonText: "SELECCIONE FECHA DE PARAMETRO",
                }).datepicker();
            });
        }

 


        //======================================================================================
        // TAB POSTBACK                                                                        
        //======================================================================================
        function TAB_ACTIVO() {
          

            HD_ORI = $("#<%=HD_TAB_ORI.ClientID%>");
            HD_DES = $("#<%=HD_TAB_DES.ClientID%>");

  
            var ObjetoStr = '#TAB_INFO #' + HD_ORI.val() + ' a[href="#' + HD_DES.val() + '"]';


            var Objeto = $(ObjetoStr);
            Objeto.tab('show');


        }


        //======================================================================================
        // FUNCION CERRAR PESTAÑA                                                                       
        //======================================================================================
        function close_window() {
            window.close();
        }






        //======================================================================================
        // SOLO NUMEROS                                                                     
        //======================================================================================
        function numbersonly(e) {

            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8 && unicode != 44) {

                if (unicode < 48 || unicode > 57) //if not a number
                {
                    { return false } //disable key press   
                }

            }

        }




    </script>
    <!-- =================================================================================================================== -->
    <!-- FIN SCRIPT                                                                                                          -->
    <!-- =================================================================================================================== -->




</asp:Content>
<asp:Content ID="Content2"  ContentPlaceHolderID="MainContent" runat="server">


    <!-- =================================================================================================================== -->
    <!-- LIMPIAR LOG                                                                                                         -->
    <!-- =================================================================================================================== -->
       <div class="row">
        <div class="col-sm-12">
            <div id="LOG_API" role="alert" style="display: none;min-height :5px;" class="alert alert-info">
                <button type="button" onclick="LIMPIAR_LOG()" ><span aria-hidden="true">&times;</span>
                </button>
                <strong>LOG API</strong>
                <div id="RESULTADO" ></div>
            </div>
        </div>
      </div>

   


   <hr />

    <asp:UpdatePanel ID="UPDATE_PANEL_CONTENEDOR"  runat="server" >
    <ContentTemplate>

    <asp:HiddenField ID="HD_TAB_ORI" runat="server" Value="DV_1000" />
    <asp:HiddenField ID="HD_TAB_DES" runat="server" Value="DV_0" />


    <%--==============================================================================================================--%>
    <%-- CONTAINER                                                                                                    --%>
    <%--==============================================================================================================--%>
    <div class="container-fluid">
        
<%--        <div class="row" style ="margin-top :5px;"></div>--%>
     <%--   <div class="row">
            <div class="col-sm-12">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item active">FORMULARIO</li>
                </ol>
            </div>
        </div>--%>
        
        <%--===========================================================================================================--%>
        <%--===========================================================================================================--%>
	<%--	<div class="row" style ="margin-top :5px;"></div>--%>

        <%--===========================================================================================================--%>
        <%--===========================================================================================================--%>
   <%--     <div class="row">
            <div class="col-sm-12">
                <hr />
            </div>
        </div>--%>
        
        <div class="row">
            <div class="col-sm-12">
                 <div id="PNL_DINAMICO" runat="server">

                 </div>
            </div>
        </div>
	
    </div>
    <%--==============================================================================================================--%>
    <%-- FIN CONTAINER                                                                                                --%>
    <%--==============================================================================================================--%>

        </ContentTemplate>
    </asp:UpdatePanel>
        
</asp:Content>



