$(function () {
    init();

    $('.opButtonDetail').click(function (e) {
        let selectedKey = this.id.replace('DetailItem_', '');
        $('#SelectedKey').val(selectedKey);
        $("#formList").data('SelectedKey',selectedKey);
        let moduleName = $(this).data('module-name');
        let actionName = "DetailItem";
        openModalPopup(moduleName, actionName);
    });

    $('.opButtonInsert').click(function (e) {
        let actionName = "InsertItem";
        let moduleName = $(this).data('module-name');
        openModalPopup(moduleName, actionName);
    });

    $('.opButtonDelete').click(function (e) {
        let selectedKey = this.id.replace('DeleteItem_', '');
        $('#SelectedKey').val(selectedKey);
        $("#formList").data('SelectedKey',selectedKey);
        let moduleName = $(this).data('module-name');
        let actionName = "DeleteItem";
        openModalPopup(moduleName, actionName);
    });

    $('.opButtonUpdate').click(function (e) {
        let selectedKey = this.id.replace('UpdateItem_', '');
        $('#SelectedKey').val(selectedKey);
        $("#formList").data('SelectedKey',selectedKey);
        let moduleName = $(this).data('module-name');
        let actionName = "UpdateItem";
        openModalPopup(moduleName, actionName);
    });

    function openModalPopup(moduleName, actionName) {
        let popupUrl = `/${moduleName}/${actionName}`;

        $.ajax({
            type: "POST",
            url: popupUrl,
            processData: false,
            data: $("#formList").serialize(),
            success: function (data) {
                $('#itemDetailsContainer').html(data);
                $('#itemDetailsModal').modal('show');
                loadJsonAllEditors();
                loadYamlAllEditors();
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
        let url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        let form = $(this).parents('.modal').find('form');
        let actionUrl = form.attr('action');
        let dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            let isValid = newBody.find('[name="IsValid"]').val() === 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
            }
        });
    });

    $(document).on('click', '[data-toggle="lightbox"]', function (event) {
        event.preventDefault();
        let targetId = '';
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
    let jsonEditors = $('.jsoneditor-class');
    jsonEditors.each(function (index) {
        const itemName = jsonEditors[index].id;
        const jsonReadonlyPrefix = "jsonEditorRO_";
        const isReadonly = itemName.startsWith(jsonReadonlyPrefix);
        const jsonPrefix = isReadonly ? jsonReadonlyPrefix : jsonReadonlyPrefix.replace('RO_', '_');
        loadJsonEditor(itemName, itemName.replace(jsonPrefix, ''), isReadonly);
    });
}

function loadYamlAllEditors() {
    let jsonEditors = $('.yamleditor-class');
    jsonEditors.each(function (index) {
        const itemName = jsonEditors[index].id;
        const jsonReadonlyPrefix = "yamlEditorRO_";
        const isReadonly = itemName.startsWith(jsonReadonlyPrefix);
        let editor = ace.edit(itemName);
        editor.session.setMode("ace/mode/yaml");
        editor.setTheme("ace/theme/github");
        editor.setReadOnly(isReadonly);
    });
}

function addYamlEditorsToFormData(formData) {
    const yamlEditorPrefix = 'yamlEditor_';
    const yamlEditors = document.querySelectorAll(`div[id^='${yamlEditorPrefix}']`);

    yamlEditors.forEach(editor => {
        const editorId = editor.id.replace(yamlEditorPrefix, '').replace('_','.');
        const aceEditor = ace.edit(editor.id);
        const editorContent = aceEditor.getValue().trim();
        formData.append(editorId, editorContent);
    });
}

function setPage(pageId) {
    $('#CurrentPage').val(pageId);
    $('#formList').submit();
}

function loadJsonEditor(jsonEditorName, jsonDataItem, isReadonly) {
    const container = document.getElementById(jsonEditorName);
    let modes = ['code', 'text', 'tree'];
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
    let jsonValue = $('#' + jsonDataItem).val();
    let emptyJson = "{}";

    if (jsonValue.length === 0) {
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
    let globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}

