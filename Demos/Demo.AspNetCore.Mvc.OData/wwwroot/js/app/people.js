'use strict'

class ViewModel {
    constructor() {
        this.apiUrl = "/odata/PersonApi";

        this.validator = false;

        this.id = ko.observable(0);
        this.familyName = ko.observable(null);
        this.givenNames = ko.observable(null);
        this.dateOfBirth = ko.observable(null);
    }

    init = () => {
        currentSection = $("#grid-section");

        this.validator = $("#form-section-form").validate({
            rules: {
                FamilyName: { required: true, maxlength: 128 },
                GivenNames: { required: true, maxlength: 128 },
                DateOfBirth: { required: true, date: true }
            }
        });

        $("#grid").kendoGrid({
            data: null,
            dataSource: {
                type: 'odata',
                transport: {
                    read: {
                        url: this.apiUrl,
                        dataType: "json"
                    },
                    parameterMap: function (options, operation) {
                        let paramMap = kendo.data.transports.odata.parameterMap(options);
                        if (paramMap.$inlinecount) {
                            if (paramMap.$inlinecount == "allpages") {
                                paramMap.$count = true;
                            }
                            delete paramMap.$inlinecount;
                        }
                        if (paramMap.$filter) {
                            paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                        }
                        return paramMap;
                    }
                },
                schema: {
                    data: function (data) {
                        return data.value;
                    },
                    total: function (data) {
                        return data["@odata.count"];
                    },
                    model: {
                        fields: {
                            FamilyName: { type: "string" },
                            GivenNames: { type: "string" },
                            DateOfBirth: { type: "date" }
                        }
                    }
                },
                pageSize: 10,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                sort: [
                    { field: "FamilyName", dir: "asc" },
                    { field: "GivenNames", dir: "asc" }
                ]
            },
            dataBound: function (e) {
                let body = this.element.find("tbody")[0];
                if (body) {
                    ko.cleanNode(body);
                    ko.applyBindings(ko.dataFor(body), body);
                }
            },
            filterable: true,
            sortable: {
                allowUnsort: false
            },
            pageable: {
                refresh: true
            },
            scrollable: false,
            columns: [{
                field: "FamilyName",
                title: "Family Name"
            }, {
                field: "GivenNames",
                title: "Given Name/s"
            }, {
                field: "DateOfBirth",
                title: "Date of Birth",
                type: "date",
                format: "{0:yyyy-MM-dd}",
                filterable: false,
                width: 200
            }, {
                field: "Id",
                title: "&nbsp;",
                template: '<div class="btn-group">' +
                    '<button type="button" data-bind="click: edit.bind($data,\'#=Id#\')" class="btn btn-default btn-sm" title="Edit"><i class="fa fa-edit"></i></button>' +
                    '<button type="button" data-bind="click: remove.bind($data,\'#=Id#\')" class="btn btn-danger btn-sm" title="Delete"><i class="fa fa-remove"></i></button>' +
                    '</div>',
                attributes: { "class": "text-center" },
                filterable: false,
                width: 100
            }]
        });

        $("#DateOfBirth").kendoDatePicker({
            start: "decade",
            format: "yyyy-MM-dd"
        });
    };

    create = () => {
        this.id(0);
        this.familyName(null);
        this.givenNames(null);
        this.dateOfBirth(null);

        this.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html("Create");
    };

    edit = async (id) => {
        const data = await ApiHelper.getRecord(`${this.apiUrl}(${id})`);
        this.id(data.Id);
        this.familyName(data.FamilyName);
        this.givenNames(data.GivenNames);
        this.dateOfBirth(data.DateOfBirth);

        switchSection($("#form-section"));
        $("#form-section-legend").html("Edit");
    };

    remove = async (id) => {
        await ApiHelper.deleteRecord(`${this.apiUrl}(${id})`);
    };

    save = async () => {
        const isNew = (this.id() == 0);

        if (!$("#form-section-form").valid()) {
            return false;
        }

        const record = {
            Id: this.id(),
            FamilyName: this.familyName(),
            GivenNames: this.givenNames(),
            DateOfBirth: this.dateOfBirth()
        };

        if (isNew) {
            await ApiHelper.postRecord(this.apiUrl, record);
        }
        else {
            await ApiHelper.putRecord(`${this.apiUrl}(${this.id()})`, record);
        }
    };

    cancel = () => {
        switchSection($("#grid-section"));
    };
}