// JavaScript функция для установки Cookie
window.cookieHelper = {
    getCookie: function (name) {
        let matches = document.cookie.match(new RegExp(
            "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    },
    setCookie: function (name, value, options = {}) {
        options = {
            path: '/',
            // add other defaults if necessary
            ...options
        };

        if (options.expires instanceof Date) {
            options.expires = options.expires.toUTCString();
        }

        let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

        for (let optionKey in options) {
            updatedCookie += "; " + optionKey;
            let optionValue = options[optionKey];
            if (optionValue !== true) {
                updatedCookie += "=" + optionValue;
            }
        }

        document.cookie = updatedCookie;
    },
    deleteCookie: function (name) {
        // Set the cookie with an expired date to delete it
        this.setCookie(name, '', { 'max-age': -1 });
    }
    
    
}

document.addEventListener('contextmenu', function(event) {
    event.preventDefault(); // Блокирует контекстное меню
});
