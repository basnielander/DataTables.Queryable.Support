﻿
var popup, dataTable;

$(document).ready(function () {
    dataTable = $('#gridTodo').DataTable({
        "pageLength": 5,
        "lengthChange": false,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/api/todo",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "description" },
            { "data": "createdBy" },
            { "data": "estimatedDuration" },
            //{ "data": "deadline" },
            //{ "data": "expectedCosts" },
            //{ "data": "finished" },
            {
                "data": "id",
                "render": function (data) {
                    return "<a class='btn btn-default btn-sm' onclick=ShowPopup('/Home/AddEditTodo/" + data +
                        "')><i class='fa fa-pencil'></i> Edit</a><a class='btn btn-danger btn-sm' style='margin-left:5px' onclick=Delete(" + data +
                        ")><i class='fa fa-trash'></i> Delete</a>";
                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "initComplete": function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<input type="text"/>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val, true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>');
                });
            });
        }
    });    
});

function ShowPopup(url) {
    var formDiv = $('<div/>');
    $.get(url)
        .done(function (response) {
            formDiv.html(response);
            popup = formDiv.dialog({
                autoOpen: true,
                resizeable: false,
                width: 600,
                height: 400,
                title: 'Add or Edit Data',
                close: function () {
                    popup.dialog('destroy').remove();
                }
            });
        });
}


function SubmitAddEdit(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/api/todo',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {
                    popup.dialog('close');
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                }
            }
        });

    }
    return false;
}

function Delete(id) {
    swal({
        title: "Are you sure want to Delete?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: '/api/todo/' + id,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                }
            }
        });
    });


}


function ShowMessage(msg) {
    toastr.success(msg);
}

