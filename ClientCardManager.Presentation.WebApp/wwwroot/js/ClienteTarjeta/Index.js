$(document).ready(function () {

    const table = $("#tablaClienteTarjeta");

    $("#modalClienteTarjeta").on("input", ".numero-tarjeta", function (e) {
        $(this).mask('0000-0000-0000-0000');
    });
    

    $(".select2").select2({
        theme: "classic",
        maximumSelectionLength: 2,
        tags: false,
        tokenSeparators: [',',],
        placeholder: 'Selecciona una opción existente',
        language: {
            noResults: function () {
                return 'No se encontraron resultados';
            }
        },
    });

    $("#btnCrear").click(function () {
        let id = $("#selectCliente").val();

        if (id == 0) {

            Swal.fire({
                icon: 'warning',
                title: "Atención!!!",
                text: "Debes seleccionar un cliente antes de asociar una tarjeta.",
                showConfirmButton: true,
                timer: 4000,

            });

            return;
        }
        $("#modalClienteTarjeta").load("/ClienteTarjeta/Crear/" + id, function () {
            $("#ClienteTarjetaModal").modal("show");           
        });
    });


    $("#tablaClienteTarjeta").on("click", ".btnEditar", function (e) {
        $("#modalClienteTarjeta").load("/ClienteTarjeta/Editar/" + $(this).attr("asp-data-id"), function () {
            $("#ClienteTarjetaModal").modal("show");
        });
    });

    $("#selectCliente").on("change", function (e) {

        let id = $(this).val();
        $("#container").after($(".TopMidDivTable"))
        $("#container").after($(".TopDivTable"))
        cargarTabla(id)
    });

    function cargarTabla(id) {

        let data = {
            idCliente: id
        };

        $(table).DataTable({
            bLengthChange: false,
            responsive: true,
            destroy: true,
            scrollX: false,
            pageLength: 5,
            language: {
                processing: "Procesando",
                search: "Buscar Tarjeta:",
                lengthMenu: "Ver _MENU_ Filas",
                info: "_START_ - _END_ de _TOTAL_ elementos",
                infoEmpty: "0 - 0 de 0 elementos",
                infoFiltered: "(Filtro de _MAX_ entradas en total)",
                infoPostFix: "",
                loadingRecords: "Cargando datos...",
                zeroRecords: "No se encontraron datos",
                emptyTable: "No hay datos disponibles",
                paginate: {
                    first: "Primero",
                    previous: "Anterior",
                    next: "Siguiente",
                    last: "Ultimo"
                },
                aria: {
                    sortAscending: ": activer pour trier la colonne par ordre croissant",
                    sortDescending: ": activer pour trier la colonne par ordre décroissant"
                }
            },
            "ajax": {
                "url": finder.getAppFile("ClienteTarjeta/ObtenerClienteTarjetas"),
                "type": "get",
                data:data,
                "datatype": "json",
                "async": true,
                error: function (jqXHR, textStatus, errorThrown) {

                    console.error("Error en la solicitud:", textStatus, errorThrown);
                    $(table).DataTable().clear().draw();

                    Swal.fire({
                        icon: 'warning',
                        title: "Atención!!!",
                        text: jqXHR.responseText,
                        showConfirmButton: true,
                        timer: 2500,

                    });

                }
            },
            "columns": [
                { "data": "nombreCompleto", },
                { "data": "tipoTarjeta", },
                { "data": "numero" },
                { "data": "banco" },
                {
                    data: "id",                    
                    render: function (data, type, row, meta) {

                        return `<button  title="Editar ClienteTarjeta" type="button" class="btn btn-warning text-light btnEditar" asp-data-id="${row['id']}" ><i class="fa-solid fa-pencil"></i> Editar</button>
`;
                    }
                }                
            ],           
            "order": [[0, 'asc']]
        });
        ajustarTop();
    }
    function ajustarTop() {
        let topRow = $("<div></div>").addClass("row mb-4 top_row");
        $("#container div:first").prepend(topRow);
        $(".top_row").prepend($(".dataTables_filter ").addClass("col-4").addClass("offset-2"));
        $(".top_row").prepend($(".TopMidDivTable"));
        $(".top_row").prepend($(".TopDivTable"));
    }


    cargarTabla($("#selectCliente").val());
    

});




