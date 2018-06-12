export class Index {
    assemblies = [];

    constructor() {
        let self = this;

        $.ajax({
            dataType: "json",
            url: "/js/assemblies.json",
            async: false,
            success: function (json) {
                $.each(json, function (i) {
                    self.assemblies.push(this);
                });
            }
        });
    }
}