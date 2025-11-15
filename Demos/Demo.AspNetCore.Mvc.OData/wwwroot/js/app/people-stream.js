'use strict';

class PeopleStreamViewModel {
    constructor(streamUrl) {
        this.streamUrl = streamUrl;
        this.grid = null;
        this.recordCount = 0;
        this.isStreaming = false;
        this.abortController = null;
    }

    init = () => {
        this.grid = $("#stream-grid")
            .kendoGrid({
                dataSource: {
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                FamilyName: { type: "string" },
                                GivenNames: { type: "string" },
                                DateOfBirth: { type: "date" }
                            }
                        }
                    }
                },
                sortable: true,
                scrollable: true,
                height: 500,
                columns: [
                    { field: "FamilyName", title: "Family Name" },
                    { field: "GivenNames", title: "Given Name/s" },
                    {
                        field: "DateOfBirth",
                        title: "Date of Birth",
                        format: "{0:yyyy-MM-dd}",
                        width: 180
                    }
                ]
            })
            .data("kendoGrid");

        $("#start-stream").on("click", this.startStreaming);
        $("#stop-stream").on("click", this.stopStreaming);
        $("#reset-stream").on("click", this.resetGrid);
    };

    startStreaming = async () => {
        if (this.isStreaming) {
            return;
        }

        this.resetGrid();
        this.recordCount = 0;
        this.isStreaming = true;
        this.abortController = new AbortController();
        $("#stop-stream").prop("disabled", false);
        this.updateStatus("Streaming people ...", "info");

        try {
            const response = await fetch(this.streamUrl, { signal: this.abortController.signal });
            if (!response.ok || !response.body) {
                throw new Error(`Streaming request failed with status ${response.status}`);
            }

            const reader = response.body.getReader();
            const decoder = new TextDecoder();
            let buffer = "";

            while (true) {
                const { value, done } = await reader.read();
                if (done) {
                    break;
                }

                buffer += decoder.decode(value, { stream: true });
                buffer = this.processBuffer(buffer);
            }

            buffer += decoder.decode();
            this.processBuffer(buffer, true);

            this.updateStatus(`Streaming completed. Loaded ${this.recordCount} records.`, "success");
        } catch (error) {
            if (error.name === "AbortError") {
                this.updateStatus(`Streaming cancelled after ${this.recordCount} records.`, "warning");
            } else {
                console.error(error);
                this.updateStatus(error.message ?? "Streaming failed.", "danger");
            }
        } finally {
            this.isStreaming = false;
            $("#stop-stream").prop("disabled", true);
            this.abortController = null;
        }
    };

    stopStreaming = () => {
        if (!this.abortController) {
            return;
        }

        this.abortController.abort();
    };

    resetGrid = () => {
        if (this.grid) {
            this.grid.dataSource.data([]);
        }

        this.recordCount = 0;
        $("#record-count").text(this.recordCount);
        this.updateStatus("Stream cleared. Click Start Streaming to begin.", "info");
    };

    processBuffer = (buffer, isFinal = false) => {
        let newlineIndex = buffer.indexOf("\n");

        while (newlineIndex >= 0) {
            const line = buffer.substring(0, newlineIndex).trim();
            buffer = buffer.substring(newlineIndex + 1);
            this.addRecordFromLine(line);
            newlineIndex = buffer.indexOf("\n");
        }

        if (isFinal) {
            this.addRecordFromLine(buffer.trim());
            return "";
        }

        return buffer;
    };

    addRecordFromLine = (line) => {
        if (!line) {
            return;
        }

        try {
            const record = JSON.parse(line);
            this.grid.dataSource.add(record);
            this.recordCount += 1;
            $("#record-count").text(this.recordCount);
        } catch (error) {
            console.error("Unable to parse line", line, error);
        }
    };

    updateStatus = (message, intent) => {
        $("#stream-status")
            .removeClass("alert-info alert-success alert-danger alert-warning")
            .addClass(`alert-${intent}`)
            .text(message);
    };
}

