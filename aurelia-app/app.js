import 'jquery';
import { HttpClient } from 'aurelia-http-client';
import { PLATFORM } from 'aurelia-pal';

export class App {
    async configureRouter(config, router) {
        config.title = 'Extenso';
		config.map([
			{ moduleId: PLATFORM.moduleName('aurelia-app/index'), route: ['', 'index'], name: 'index', nav: true, title: 'Home' },
			{ moduleId: PLATFORM.moduleName('aurelia-app/assembly'), route: 'assembly/:id', name: 'assembly', title: 'Assembly' }
		]);
        this.router = router;
    }
}