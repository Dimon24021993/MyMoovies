const config = {
    get baseApiUrl() {
        switch (window.location.hostname.toLowerCase()) {
            case "localhost":
                return "https://localhost:44379/api";
            case "mymoovies.ga":
                return "https://api.mymoovies.ga/api";
        }
    },
}

export default config;