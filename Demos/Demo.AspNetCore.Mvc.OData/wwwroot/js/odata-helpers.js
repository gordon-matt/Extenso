class ODataHelper {
    static #defaultOptions = {
        refreshODataGrid: this.refreshODataGrid,
        messages: {
            getRecordError: "Error when trying to retrieve record!",
            deleteRecordConfirm: "Are you sure that you want to delete this record?",
            deleteRecordSuccess: "Successfully deleted record!",
            deleteRecordError: "Error when trying to retrieve record!",
            insertRecordSuccess: "Successfully inserted record!",
            insertRecordError: "Error when trying to insert record!",
            updateRecordSuccess: "Successfully updated record!",
            updateRecordError: "Error when trying to update record!"
        }
    };

    static options = this.#defaultOptions;

    static async getOData(url) {
        return await fetch(url)
            .then(response => response.json())
            .catch(error => {
                $.notify({ message: this.options.messages.getRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
                console.error('Error: ', error);
            });
    }

    static async deleteOData(url) {
        if (confirm(this.options.messages.deleteRecordConfirm)) {
            await fetch(url, { method: 'DELETE' })
                .then(response => {
                    if (response.ok) {
                        this.options.refreshODataGrid();
                        $.notify({ message: this.options.messages.deleteRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
                    } else {
                        $.notify({ message: this.options.messages.deleteRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
                    }
                })
                .catch(error => {
                    $.notify({ message: this.options.messages.deleteRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
                    console.error('Error: ', error);
                });
        }
    }

    static async postOData(url, record) {
        return await fetch(url, {
            method: "POST",
            headers: {
                'Content-type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(record)
        })
        .then(response => {
            if (response.ok) {
                this.options.refreshODataGrid();
                switchSection($("#grid-section"));
                $.notify({ message: this.options.messages.insertRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.options.messages.insertRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
            return response;
        })
        .catch(error => {
            $.notify({ message: this.options.messages.insertRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            console.error('Error: ', error);
        });
    }

    static async putOData(url, record) {
        return await fetch(url, {
            method: "PUT",
            headers: {
                'Content-type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(record)
        })
        .then(response => {
            if (response.ok) {
                this.options.refreshODataGrid();
                switchSection($("#grid-section"));
                $.notify({ message: this.options.messages.updateRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.options.messages.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
            return response;
        })
        .catch(error => {
            $.notify({ message: this.options.messages.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            console.error('Error: ', error);
        });
    }

    static async patchOData(url, patch) {
        return await fetch(url, {
            method: "PATCH",
            headers: {
                'Content-type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(patch)
        })
        .then(response => {
            if (response.ok) {
                this.options.refreshODataGrid();
                $.notify({ message: this.options.messages.updateRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.options.messages.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
            return response;
        })
        .catch(error => {
            $.notify({ message: this.options.messages.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            console.error('Error: ', error);
        });
    }

    static refreshODataGrid() {
        $('#grid').data('kendoGrid').dataSource.read();
        $('#grid').data('kendoGrid').refresh();
    };
}