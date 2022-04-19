function display_message(title, message, type, delay) {
    var options = {
        text: ""
    };
    options.title = title;
    options.text = message;
    options.type = type;
    options.hide = true;
    options.delay = delay;
    options.buttons = {
        closer: true,
        sticker: true
    };
    if (type == "success")
        options.icon = 'fa fa-check';
    else if (type == "error")
        options.icon = 'fa fa-cancel';
    else if (type == "info")
        options.icon = 'fas fa-info-circle';

    options.shadow = true;
    options.width = '250px';

    stack_context_modal = {
        "dir1": "down",
        "dir2": "left",
        "modal": true,
        "overlay_close": true
    };

    new PNotify({
        title: title,
        text: message,
        type: type,
        addclass: "stack-modal",
        stack: stack_context_modal,
        icon: options.icon,
        width: "250px",
        delay: options.delay,
        stack: stack_context_modal
    });
}