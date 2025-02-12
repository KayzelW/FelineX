export class AceEditor {
    static initialize(dotNetObject, editorId) {
        console.log("start init ace with id" + editorId)
        var editor = ace.edit(editorId);
        editor.setTheme("ace/theme/textmate");
        editor.session.setMode("ace/mode/sql");

        // Установка размера шрифта
        editor.setFontSize("16px");

        // Загрузка модулей автозавершения
        ace.config.loadModule('ace/ext/language_tools', function() {
            editor.setOptions({
                enableBasicAutocompletion: true,
                enableLiveAutocompletion: true,
                enableSnippets: true,
                showLineNumbers: true,
                tabSize: 4
            });

        });

        // Обработка изменений текста
        editor.on('change', function() {
            var value = editor.getValue();
            dotNetObject.invokeMethodAsync('UpdateValue', value);
        });
    };
    static getText(editorId) {
        var editor = ace.edit(editorId);
        return editor.getValue();
    };

    static setText(editorId, value) {
        var editor = ace.edit(editorId);
        editor.setValue(value, -1);
    };
}

window.AceEditor = AceEditor;
