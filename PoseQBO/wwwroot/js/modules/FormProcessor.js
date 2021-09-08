class FormProcessor {
    constructor(form, formStateBase) {
        this.form = form;
        this.formStateBase = formStateBase;
    }

    setFormState() {
        this.form.innerHTML = this.formStateBase.State;
    }

    setActionAttributeValue(value) {
        this.form.setAttribute('action', value);
    }
}

export default FormProcessor;