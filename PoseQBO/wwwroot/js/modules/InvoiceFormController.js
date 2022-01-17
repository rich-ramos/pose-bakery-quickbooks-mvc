import LoadingSpinner from "./LoadingSpinner.js";

class InvoiceFormController {
    #formControlErrorClass = 'is-invalid';

    constructor() {
        this.form = document.querySelector('#invoiceForm');
        this.invalidFormFields = [];
        this.events();
        this.loadingSpinner = new LoadingSpinner();
    }

    events() {
        this.form.addEventListener('submit', (e) => this.handleSubmit(e));
    }

    handleSubmit(e) {
        const startDate = document.querySelector('#startDate');
        const endDate = document.querySelector('#endDate');
        const companyName = document.querySelector('#companyName');
        let isValidDates = this.#validateStartDateIsNotGreaterThanEndDate(startDate.value, endDate.value);

        if (!isValidDates) {
            this.invalidFormFields.push(startDate, endDate);
        }

        if (this.form.contains(companyName)) {
            let isValidName = this.#isCompanyNameValid(companyName.value);
            if (!isValidName) {
                this.invalidFormFields.push(companyName);
            }
        }

        if (this.invalidFormFields.length > 0) {
            this.#addIsInvalidClassToFormFields(this.invalidFormFields);
            this.#addClickEventListenerToFormFields(this.invalidFormFields);
            this.#clearInvalidFromFieldsArray();
            e.preventDefault();
            return;
        }
        this.loadingSpinner.showSpinner();
    }

    handleClick(e) {
        const formFieldElement = e.target;
        if (formFieldElement.classList.contains(this.#formControlErrorClass)) {
            formFieldElement.classList.remove(this.#formControlErrorClass)
        }
    }

    #addClickEventListenerToFormFields(invalidFormFields) {
        invalidFormFields.forEach((invalidFromField) => {
            invalidFromField.addEventListener('click', (e) => this.handleClick(e));
        });
    }


    #clearInvalidFromFieldsArray() {
        this.invalidFormFields.length = 0;
    }

    #isCompanyNameValid(companyName) {
        const re = /^[a-zA-Z0-9]{1,40}/;
        return (!re.test(companyName)) ? false : true;
    }

    #validateStartDateIsNotGreaterThanEndDate(startDate, endDate) {
        const sDate = new Date(startDate);
        const eDate = new Date(endDate);
        let isValid = true;
        if (sDate.getTime() > eDate.getTime() || sDate.getTime() == eDate.getTime()) {
            isValid = false;
        }
        return isValid;
    }

    #addIsInvalidClassToFormFields(formFields) {
        formFields.forEach((field) => {
            field.classList.add(this.#formControlErrorClass);
        });
    }
}

export default InvoiceFormController;