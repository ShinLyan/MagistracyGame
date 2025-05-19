mergeInto(LibraryManager.library, {
    DownloadFile: function (filename, fileData) {
        console.log("DownloadFile called with filename: " + UTF8ToString(filename));

        var link = document.createElement('a');
        var dataUrl = 'data:image/png;base64,' + UTF8ToString(fileData);

        console.log("Data URL: " + dataUrl.substring(0, 50));

        link.href = dataUrl;
        link.download = UTF8ToString(filename);

        try {
            link.click();
        } catch (e) {
            console.error("Error while downloading: " + e.message);
        }
    }
});
