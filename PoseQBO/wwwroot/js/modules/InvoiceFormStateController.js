import DefaultInvoiceFormState from "../invoice-form-state/component/DefaultInvoiceFormState.js";
import FormProcessor from "./FormProcessor.js";
import invoiceFormStateDecoratorFactory from './../invoice-form-state/decorators/factory/invoiceFormStateDecoratorFactory.js';

class InvoiceFormStateController {

    constructor() {
        this.form = document.querySelector('#invoiceForm');
        this.defaultInvoiceFormState = new DefaultInvoiceFormState();
        this.formProcessor = null;
        this.invoiceNavLinks = document.querySelectorAll('.invoice-nav-link');
        this.events();
    }

    events() {
        document.addEventListener('DOMContentLoaded', (e) => this.initializeDefaultState(e));
        this.invoiceNavLinks.forEach((invoiceNavLink) => {
            invoiceNavLink.addEventListener('click', (e) => this.handleClick(e));
        });
    }

    handleClick(e) {
        this.invoiceNavLinks.forEach((invoiceNavLink) => {
            invoiceNavLink.classList.remove('active');
        });
        e.target.classList.add('active');
        this.changeFormState(e.target.getAttribute('data-state'));
    }

    changeFormState(state) {
        let invoiceFormStateDecoratorBase = invoiceFormStateDecoratorFactory.createInvoiceFormStateDecorator(state, this.defaultInvoiceFormState);
        this.formProcessor = new FormProcessor(this.form, invoiceFormStateDecoratorBase);
        this.formProcessor.setFormState();
        this.formProcessor.setActionAttribute();
    }

    initializeDefaultState(e) {
        this.formProcessor = new FormProcessor(this.form, this.defaultInvoiceFormState);
        this.formProcessor.setFormState();
        this.formProcessor.setActionAttribute();
    }
}

export default InvoiceFormStateController;