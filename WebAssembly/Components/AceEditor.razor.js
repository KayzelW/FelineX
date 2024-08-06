export class AceEditor {
    static initialize() {
        var editor = ace.edit("editor");
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

            // Включение подсветки синтаксических ошибок
            ace.config.loadModule('ace/ext/error_marker', function() {
                editor.getSession().setUseWorker(true);
            });
        });
    }
}

window.AceEditor = AceEditor;