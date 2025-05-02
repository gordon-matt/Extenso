'use strict'

var ViewModel = function () {
    var self = this;

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
                transport: {
                    read: {
                        type: 'post',
                        dataType: 'json',
                        url: `/people/grid`
                    }
                },
                schema: {
                    data: 'data',
                    total: 'total',
                    model: {
                        fields: {
                            familyName: { type: "string" },
                            givenNames: { type: "string" },
                            dateOfBirth: { type: "date" }
                        }
                    }
                },
                pageSize: 10,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                sort: [
                    { field: "familyName", dir: "asc" },
                    { field: "givenNames", dir: "asc" }
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
                field: "familyName",
                title: "Family Name"
            }, {
                field: "givenNames",
                title: "Given Name/s"
            }, {
                field: "dateOfBirth",
                title: "Date of Birth",
                type: "date",
                format: "{0:yyyy-MM-dd}",
                filterable: false,
                width: 200
            }, {
                field: "id",
                title: "&nbsp;",
                template:
                    '<div class="btn-group">' +
                        '<button type="button" data-bind="click: edit.bind($data,\'#=id#\')" class="btn btn-default btn-sm" title="Edit"><i class="fa fa-edit"></i></button>' +
                        '<button type="button" data-bind="click: remove.bind($data,\'#=id#\')" class="btn btn-danger btn-sm" title="Delete"><i class="fa fa-remove"></i></button>' +
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
        const data = await ApiHelper.getRecord(`/people/${id}`);
        self.id(data.id);
        self.familyName(data.familyName);
        self.givenNames(data.givenNames);
        self.dateOfBirth(data.dateOfBirth);

        switchSection($("#form-section"));
        $("#form-section-legend").html("Edit");
    };

    self.remove = async function (id) {
        await ApiHelper.deleteRecord(`/people/${id}`);
    };

    self.save = async function () {
        const isNew = (self.id() == 0);

        if (!$("#form-section-form").valid()) {
            return false;
        }

        const record = {
            id: self.id(),
            familyName: self.familyName(),
            givenNames: self.givenNames(),
            dateOfBirth: self.dateOfBirth()
        };

        if (isNew) {
            await ApiHelper.postRecord(`/people`, record);
        }
        else {
            await ApiHelper.putRecord(`/people/${self.id()}`, record);
        }
    };

    self.cancel = function () {
        switchSection($("#grid-section"));
    };
};