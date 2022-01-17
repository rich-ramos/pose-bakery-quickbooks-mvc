class LoadingSpinner {
    constructor() {
        this.spinner = document.querySelector('.lds-ring');
        this.overlay = document.querySelector('.overlay');
        this.#events();
    }

    #events() {
        document.addEventListener('DOMContentLoaded', (e) => this.#handleContentLoaded(e));
    }

    #handleContentLoaded(e) {
        this.hideSpinner();
    }

    hideSpinner() {
        this.spinner.style.display = 'none';
        this.overlay.classList.remove('overlay');
    }

    showSpinner() {
        this.spinner.style.display = 'block';
        this.overlay.classList.add('overlay');
    }
}
export default LoadingSpinner;