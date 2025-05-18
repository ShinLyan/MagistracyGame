mergeInto(LibraryManager.library, {
    DownloadFile: function (filename, fileData) {
        var link = document.createElement('a');
        link.href = 'data:application/octet-stream;base64,' + fileData;
        link.download = UTF8ToString(filename);
        link.click();
    }
});
