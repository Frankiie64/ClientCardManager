// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var finder = (function () {
    var $body = $('body');

    return {
        getAppFile: getAppFile,
    };

    function getAppFile(file) {
        var base = $body.data('root');
        return base + file;
    }
})();