export class Index {
    assemblies = [];

    constructor() {
        let self = this;
		
		let url = location.pathname;
		let urlBase = url.substring(0, url.lastIndexOf('/') + 1);

        $.ajax({
            dataType: "json",
            url: urlBase + "js/assemblies.json",
            async: false,
            success: function (json) {
                $.each(json, function (i) {
                    self.assemblies.push(this);
                });
            }
        });
    }
}