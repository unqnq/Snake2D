mergeInto(LibraryManager.library, {
    IsMobileBrowser: function() {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.indexOf('android') > -1 || ua.indexOf('iphone') > -1 || ua.indexOf('ipad') > -1) {
            return 1;
        }
        return 0;
    }
});
