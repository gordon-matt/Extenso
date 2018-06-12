import { LogManager, PLATFORM, ViewLocator } from "aurelia-framework";
import { ConsoleAppender } from "aurelia-logging-console";
import 'jquery';
import 'bootstrap';

LogManager.addAppender(new ConsoleAppender());
LogManager.setLevel(LogManager.logLevel.debug);

export function configure(aurelia) {
    aurelia.use
        .standardConfiguration()
        .developmentLogging();

    aurelia.start().then(a => a.setRoot("aurelia-app/app", document.body));
}