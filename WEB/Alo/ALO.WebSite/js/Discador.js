
//======================================================================================
// SLEEP                                                                              ==
//======================================================================================
function sleep(milliseconds) {

    LOG_API("TIEMPO MS..." + milliseconds)

    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

//======================================================================================
// LLAMAR A MITROL                                                                    ==
//======================================================================================
function LlamarApi(idInteraccion, idNumero, numero, nombre, idCampania) {


    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (REALIZANDO NRO: " + numero + "...)";


    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: idInteraccion,
        idNumero: idNumero,
        numero: numero,
        nombre: nombre,
        idCampania: idCampania
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/llamar?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false


        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO " + MENSAJE);
            LOG_API("DELAY INTERACCION...")
            sleep(500);
            ObtenerInfoInteracciones(numero);

        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}

//======================================================================================
// LLAMAR A MITROL                                                                    ==
//======================================================================================
function LlamarApiTra(idInteraccion, idNumero, numero, nombre, idCampania) {


    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (REALIZANDO NRO: " + numero + "...)";


    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: idInteraccion,
        idNumero: idNumero,
        numero: numero,
        nombre: nombre,
        idCampania: idCampania
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/llamar?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false


        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);
            LOG_API("DELAY INTERACCION...")
            sleep(500);
            ObtenerInfoInteraccionesTra(numero);


        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}
//======================================================================================
// COLGAR  MITROL                                                                     ==
//======================================================================================
function ColgarApi(idInteraccion) {




    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (COLGANDO LLAMADA...) ";



    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: encodeURIComponent(idInteraccion)
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/cortar?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false
        


        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);

        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}

//======================================================================================
// CERRAR GESTION MITROL                                                              ==
//======================================================================================
function CerrarApi(idInteraccion) {



    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (CERRANDO GESTIÓN...) ";



    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: encodeURIComponent(idInteraccion)
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/cerrar?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false


        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);

        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}
//======================================================================================
// ESTADO DISPONIBLE                                                                  ==
//======================================================================================
function EstadoIPPAD(idEstadoAgente) {



    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (ESTADO DISPONIBLE...) ";



    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idEstadoAgente: encodeURIComponent(idEstadoAgente)
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/SetEstadoAgente?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false


        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);

        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}
//======================================================================================
// OBTENER INTERACCIONES                                                              ==
//======================================================================================
function ObtenerInfoInteracciones(destino) {


    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (OBTENIENDO ID DE INTERACCION...) ";





    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/getEstadosInteracciones?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false



        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);
            
            LOG_API("DELAY INTERACCION")
            sleep(200);


            

            if (response.value.length > 0) {

                LOG_API("CODIGO 0");
                EnviarDatosParaGestion(response, destino);

            }
            else {

                LOG_API("API NO DEVOLVIO ESTADOS LOGICOS PARA BUSCAR INTERACCIÓN, " + MENSAJE);

            }


        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}

//======================================================================================
// OBTENER INTERACCIONES                                                              ==
//======================================================================================
function ObtenerInfoInteraccionesTra(destino) {


    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (OBTENIENDO ID DE INTERACCION...) ";





    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/getEstadosInteracciones?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false



        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);
            LOG_API("DELAY INTERACCION TRANF.")
            sleep(200);


            if (response.value.length > 0) {

                LOG_API("CODIGO 0");
                EnviarDatosParaGestionTra(response, destino);


            }
            else {

                LOG_API("API NO DEVOLVIO ESTADOS LOGICOS PARA BUSCAR INTERACCIÓN, " + MENSAJE);

            }


        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}
//======================================================================================
// GUARDAR GESTIONES DE MITROL                                                        ==
//======================================================================================
function GuardarGestionDiscador(idInteraccion, idcrm, idResultadoGestionExterno) {


    try {

  
        //=========================================================
        // LIMPIAR LOG                              
        //=========================================================
        LIMPIAR_LOG();


        //=========================================================
        // DECLARACION DE VARIABLES                                   
        //=========================================================
        var FechaHoraActual_I;
        var FechaHoraActual_F;
        var Calculo;



        //=========================================================
        // COLGAR                                   
        //=========================================================
        ColgarApi(idInteraccion);
        LOG_API("GUARDAR GESTION DE LLAMADA...")


        //=========================================================
        // ENVIAR UNA PAUSA
        //=========================================================
        FechaHoraActual_I = new Date();
        LOG_API("DELAY PAUSA COLGAR...")
        sleep(200);

        FechaHoraActual_F = new Date();
        Calculo = (FechaHoraActual_F - FechaHoraActual_I);
        LOG_API("DELAY ESPERA COLGAR MILISEGUNDOS..." + Calculo)

       


        //=========================================================
        // GUARDAR GESTION                                
        //=========================================================
        GuardarGestionMitrol(idInteraccion, idcrm, idResultadoGestionExterno);


        //=========================================================
        // ENVIAR UNA PAUSA
        //=========================================================
        FechaHoraActual_I = new Date();
        LOG_API("DELAY PAUSA GUARDAR GESTION...")
        sleep(200);

        FechaHoraActual_F = new Date();
        Calculo = (FechaHoraActual_F - FechaHoraActual_I);
        LOG_API("DELAY ESPERA GUARDAR GESTION MILISEGUNDOS..." + Calculo)

        


        //=========================================================
        // CERRAR                                   
        //=========================================================
        CerrarApi(idInteraccion);

        //=========================================================
        // ENVIAR UNA PAUSA
        //=========================================================
        FechaHoraActual_I = new Date();
        LOG_API("DELAY PAUSA CERRAR...")
        sleep(200);

        FechaHoraActual_F = new Date();
        Calculo = (FechaHoraActual_F - FechaHoraActual_I);
        LOG_API("DELAY ESPERA CERRAR MILISEGUNDOS..." + Calculo)
        

        //=========================================================
        // ESTADO                                   
        //=========================================================
        EstadoIPPAD(1);



    }
    catch (err) {
        
        LOG_API("ERROR, " + err.message);
    }

}

//======================================================================================
// GUARDAR GESTIONES DE MITROL                                                        ==
//======================================================================================
function GuardarGestionDiscadorTra(idInteraccion, idcrm, idResultadoGestionExterno,Valor) {


    try {


        //=========================================================
        // LIMPIAR LOG                              
        //=========================================================
        LIMPIAR_LOG();
        LOG_API("TRANSFIRIENDO LLAMADA...")

        //=========================================================
        // DECLARACION DE VARIABLES                                   
        //=========================================================
        var FechaHoraActual_I;
        var FechaHoraActual_F;
        var Calculo;


        


        //=========================================================
        // GUARDAR GESTION                                
        //=========================================================
        GuardarGestionMitrol(idInteraccion, idcrm, idResultadoGestionExterno);


        //=========================================================
        // ENVIAR UNA PAUSA
        //=========================================================
        FechaHoraActual_I = new Date();
        LOG_API("DELAY PAUSA GUARDAR GESTION...")
        sleep(100);

        FechaHoraActual_F = new Date();
        Calculo = (FechaHoraActual_F - FechaHoraActual_I);
        LOG_API("DELAY ESPERA GUARDAR GESTION MILISEGUNDOS..." + Calculo)




        
        //=========================================================
        // LLAMAR
        //=========================================================
        document.getElementById("HF_Id_InteraccionTra").value = "0";
        LlamarApiTra("", "", Valor, "", "");

        


        //=========================================================
        // ONTENER ID INTERACCION                                   
        //=========================================================
        var idInteraccionDestino = document.getElementById("HF_Id_InteraccionTra").value;
        LOG_API("idInteraccion :" + idInteraccion + " idInteraccionDestino :" + idInteraccionDestino);


        //=========================================================
        // TRANSFERENCIA                                  
        //=========================================================
        TransferirAInteraccion(idInteraccion, idInteraccionDestino)


        //=========================================================
        // ENVIAR UNA PAUSA
        //=========================================================
        FechaHoraActual_I = new Date();
        LOG_API("DELAY PAUSA TRANSFERENCIA API...")
        sleep(100);

        FechaHoraActual_F = new Date();
        Calculo = (FechaHoraActual_F - FechaHoraActual_I);
        LOG_API("DELAY TRANSFERENCIA MILISEGUNDOS..." + Calculo)



    }
    catch (err) {

        LOG_API("ERROR, " + err.message);
    }

}


//======================================================================================
// GUARDAR GESTIONES DE MITROL                                                        ==
//======================================================================================
function GuardarGestionMitrol(idInteraccion, idcrm, idResultadoGestionExterno) {




    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (GUARDANDO GESTIÓN DISCADOR...) ";




    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: encodeURIComponent(idInteraccion),
        idResultadoGestionExterno: encodeURIComponent(idResultadoGestionExterno),
        IdCRM: encodeURIComponent(idcrm)
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/setGestion?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false



        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);

        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}

//======================================================================================
// TRANSFERIR                                                                         ==
//======================================================================================
function TransferirAInteraccion(idInteraccion, idInteraccionDestino) {




    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (TRANSFIRIENDO...) ";




    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: encodeURIComponent(idInteraccion),
        idInteraccionDestino: encodeURIComponent(idInteraccionDestino)
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/TransferirAInteraccion?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false



        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);

        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}
//======================================================================================
// CERRAR GESTIONES DE MITROL                                                         ==
//======================================================================================
function CerrarGestionMitrol(idInteraccion) {


    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (CERRAR GESTIÓN DISCADOR...) ";




    //=============================================================
    // PARAMETROS DE FUNCION URL                                   
    //=============================================================
    var PARAMETROS = {
        idInteraccion: encodeURIComponent(idInteraccion)
    };

    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/cerrar?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        data: PARAMETROS,
        url: URL,
        dataType: "json",
        type: 'GET'



        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);


        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {



            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}

//======================================================================================
// ESTADO DE AGENTE MITROL                                                            ==
//======================================================================================
function EstadoAgenteApiMitrol() {


    //=============================================================
    // LIMPIAR LOG                                    
    //=============================================================
    LIMPIAR_LOG();

    //=============================================================
    // MENSAJE                                    
    //=============================================================
    var MENSAJE = " (ESTADO DE AGENTE DISCADOR...) ";




    //=============================================================
    // URL                                  
    //=============================================================
    var URL = 'http://127.0.0.1:8546/api/getEstadoAgente?format=json';


    //=============================================================
    // LLAMADA AJAX A SERVICIO DEBE POSEER UN DIV LLAMADO RESULTADO 
    // PARA MOSTRA AVANCES                                    
    //=============================================================
    $.ajax({
        url: URL,
        dataType: "json",
        type: 'GET',
        async: false



        //=========================================================
        //=========================================================
        , beforeSend: function () {
            LOG_API("PROCESANDO, " + MENSAJE);
        }
        //=========================================================
        // PROCESO DE TERMINADO
        //=========================================================
        , success: function (response) {

            LOG_API("REALIZADO, " + MENSAJE);
            LOG_API(response);

            if (response.code == -2) {

                document.getElementById("HF_Estado_Agente").value = "0";
            }
            else {
                document.getElementById("HF_Estado_Agente").value = "1";

            }



        }
        //=========================================================
        // ERROR
        //=========================================================
        , error: function (xhr, textStatus, thrownError) {

            document.getElementById("HF_Estado_Agente").value = "-1";

            //=====================================================
            //=====================================================
            if (xhr.status === 0) {

                LOG_API(MENSAJE + " NO EXISTE CONECCIÓN VERIFIQUE DIRECCION URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 404) {

                LOG_API(MENSAJE + " NO EXISTE REFERENCIA DE LA URL");
                return;
            }
            //=====================================================
            //=====================================================
            if (xhr.status === 500) {

                LOG_API(MENSAJE + " ERROR INTERNO DE SERVIDOR");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'parsererror') {

                LOG_API(MENSAJE + " FALLAS REQUEST JSON");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'timeout') {

                LOG_API(MENSAJE + " TIME OUT");
                return;
            }
            //=====================================================
            //=====================================================
            if (textStatus === 'abort') {
                LOG_API(MENSAJE + " AJAX REQUEST ABORTADO");
                return;
            }



            LOG_API(MENSAJE + " ERROR NO CONTROLADO");
        }

    });
}

//======================================================================================
// OBTENER ID INTERACCIÓN DE MITROL                                                   ==
//======================================================================================
function EnviarDatosParaGestion(response, destino) {

    //=============================================================
    // PARAMETROS DE FUNCION URL                                 
    //=============================================================
    var Arreglo = response.value;
    var INTERACCIONES = new Array();


    for (var i = 0; i < Arreglo.length; i++) {

        if (Arreglo[i].destino == destino) {

            INTERACCIONES.push(Arreglo[i].idInteraccion);
            LOG_API("OBTENIENDO INTERACCION RESPONSE :" + Arreglo[i].idInteraccion);
        }
        console.log("INTERACCION :" + Arreglo[i].idInteraccion);
    }

       


    if (INTERACCIONES == null || INTERACCIONES == 'undefined') {

        document.getElementById("HF_Id_Interaccion").value = "";

    }
    else {
        document.getElementById("HF_Id_Interaccion").value = INTERACCIONES;

    }



}

//======================================================================================
// OBTENER ID INTERACCIÓN DE MITROL                                                   ==
//======================================================================================
function EnviarDatosParaGestionTra(response, destino) {

    //=============================================================
    // PARAMETROS DE FUNCION URL                                 
    //=============================================================
    var Arreglo = response.value;
    var INTERACCIONES = new Array();


    for (var i = 0; i < Arreglo.length; i++) {

        if (Arreglo[i].destino == destino) {

            INTERACCIONES.push(Arreglo[i].idInteraccion);
            LOG_API("OBTENIENDO INTERACCION RESPONSE :" + Arreglo[i].idInteraccion);
        }

        console.log("INTERACCION :" + Arreglo[i].idInteraccion);
    }

    
    if (INTERACCIONES == null || INTERACCIONES == 'undefined') {

        document.getElementById("HF_Id_InteraccionTra").value = "";

    }
    else {

        document.getElementById("HF_Id_InteraccionTra").value = INTERACCIONES;

    }



}