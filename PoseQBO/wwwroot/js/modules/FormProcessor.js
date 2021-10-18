class FormProcessor {
    constructor(form, formStateBase) {
        this.form = form;
        this.formStateBase = formStateBase;
        this.invoiceState = document.querySelector('.invoice-state');
    }

    setFormState() {
        this.invoiceState.innerHTML = this.formStateBase.State;
    }

    setActionAttribute() {
        this.form.setAttribute('action', this.formStateBase.Action);
    }
}

export default FormProcessor;