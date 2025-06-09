'use strict'

var ViewModel = function () {
    var self = this;

    self.apiUrl = "/odata/PersonApi";
    
    self.validator = false;

    self.id = ko.observable(0);
    self.familyName = ko.observable(null);
    self.givenNames = ko.observable(null);
    self.dateOfBirth = ko.observable(null);

    self.init = function () {

        currentSection = $("#grid-section");
        
        self.validator = $("#form-section-form").validate({
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
                        url: self.apiUrl,
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
                template:
                    '<div class="btn-group">' +
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

    self.create = function () {
        self.id(0);
        self.familyName(null);
        self.givenNames(null);
        self.dateOfBirth(null);

        self.validator.resetForm();
        switchSection($("#form-section"));
        $("#form-section-legend").html("Create");
    };

    self.edit = async function (id) {
        const data = await ApiHelper.getRecord(`${self.apiUrl}(${id})`);
        self.id(data.Id);
        self.familyName(data.FamilyName);
        self.givenNames(data.GivenNames);
        self.dateOfBirth(data.DateOfBirth);

        switchSection($("#form-section"));
        $("#form-section-legend").html("Edit");
    };

    self.remove = async function (id) {
        await ApiHelper.deleteRecord(`${self.apiUrl}(${id})`);
    };

    self.save = async function () {
        const isNew = (self.id() == 0);

        if (!$("#form-section-form").valid()) {
            return false;
        }

        const record = {
            Id: self.id(),
            FamilyName: self.familyName(),
            GivenNames: self.givenNames(),
            DateOfBirth: self.dateOfBirth()
        };

        if (isNew) {
            await ApiHelper.postRecord(self.apiUrl, record);
        }
        else {
            await ApiHelper.putRecord(`${self.apiUrl}(${self.id()})`, record);
        }
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };
};