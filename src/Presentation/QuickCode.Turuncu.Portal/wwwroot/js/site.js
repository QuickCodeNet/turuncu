// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    init();

    $('.opButtonDetail').click(function (e) {
        var selectedKey = this.id.replace('DetailItem_', '');
        $('#SelectedKey').val(selectedKey);

        var url = "DetailItem";
        openModalPopup(url);
    });

    $('.opButtonInsert').click(function (e) {
        var url = "InsertItem";
        openModalPopup(url);
    });

    $('.opButtonDelete').click(function (e) {
        var selectedKey = this.id.replace('DeleteItem_', '');
        $('#SelectedKey').val(selectedKey);

        var url = "DeleteItem";
        openModalPopup(url);
    });

    $('.opButtonUpdate').click(function (e) {
        var selectedKey = this.id.replace('UpdateItem_', '');
        $('#SelectedKey').val(selectedKey);

        var url = "UpdateItem";
        openModalPopup(url);
    });


    function openModalPopup(url) {
        $.ajax({
            type: "POST",
            url: url,
            processData: false,
            data: $("#formList").serialize(),
            success: function (data) {
                $('#itemDetailsContainer').html(data);
                $('#itemDetailsModal').modal('show');
                loadJsonAllEditors();
            },
            error: function (xhr, textStatus, error) {
                console.log(xhr.statusText);
                console.log(textStatus);
                console.log(error);
            },
        });
    }
});

function init() {
    $('[data-toggle="popover"]').popover();
    const placeholderElement = $('#itemDetailsContainer');
    const datePicker = $('.datetimepicker-input');
    if (datePicker.length > 0) {
        datePicker.datetimepicker({
            format: 'L'
        });
    }
    
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
            }
        });
    });

    $(document).on('click', '[data-toggle="lightbox"]', function (event) {
        event.preventDefault();
        var targetId = '';
        $(this).ekkoLightbox({
            onShown: function (lb) {
                $(targetId).addClass('lbackground');
            },
            onShow: function (lb) {
                targetId = '#' + lb.delegateTarget.id
                $(targetId).addClass('lbackground');
            },
            onHidden: function () {
                $(targetId).removeClass('lbackground');
                if ($('.modal:visible').length) {
                    $('body').addClass('modal-open');
                    $('#itemDetailsModal').focus();
                }

            }
        });
    });
}

function loadJsonAllEditors() {
    var jsonEditors = $('.jsoneditor-class');
    jsonEditors.each(function (index) {
        const itemName = jsonEditors[index].id;
        const jsonReadonlyPrefix = "jsonEditorRO_";
        const isReadonly = itemName.startsWith(jsonReadonlyPrefix);
        const jsonPrefix = isReadonly ? jsonReadonlyPrefix : jsonReadonlyPrefix.replace('RO_', '_');
        loadJsonEditor(itemName, itemName.replace(jsonPrefix, ''), isReadonly);
    });
}

function setPage(pageId) {
    $('#CurrentPage').val(pageId);
    $('#formList').submit();
}

function loadJsonEditor(jsonEditorName, jsonDataItem, isReadonly) {
    const container = document.getElementById(jsonEditorName);
    var modes = ['code', 'text', 'tree'];
    const options = {
        mainMenuBar: true,
        navigationBar: true,
        statusBar: true,
        mode: 'code',
        modes: modes,
        onEditable: function (path, field, value) {
            return !isReadonly;
        },
        onChangeText: function (jsonString) {
            $('#' + jsonDataItem).val(jsonString);
        }
    }

    setJsonDataToEditor(container, options, jsonDataItem);
}

function setJsonDataToEditor(container, options, jsonDataItem) {
    const editor = new JSONEditor(container, options);
    var jsonValue = $('#' + jsonDataItem).val();
    var emptyJson = "{}";

    if (jsonValue.length == 0) {
        jsonValue = emptyJson;
    }
    try {
        const initialJson = JSON.parse(jsonValue);
        editor.set(initialJson);
    } catch {
        const initialJson = JSON.parse(emptyJson);
        editor.set(initialJson);
    }
}



$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}

