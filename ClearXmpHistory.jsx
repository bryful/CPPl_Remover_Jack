﻿/*
{
	var proj = app.project;
	// load the XMP librar y as an ExtendScript ExternalObject
	if (ExternalObject.AdobeXMPScript == undefined) {
		ExternalObject.AdobeXMPScript = new
		ExternalObject('lib:AdobeXMPScript');
	}
	var mdata = new XMPMeta(app.project.xmpPacket); // get the project’s XMP metadata
	// update the Label project metadata’s value
	var schemaNS = XMPMeta.getNamespaceURI("xmpMM");
	var propName = "xmpMM:History";
	try {
			mdata.deleteProperty(schemaNS, propName);
			var cnt2 = mdata.countArrayItems(schemaNS, propName);
	}
	catch(e) {
		alert(e.toString());
	}
	app.project.xmpPacket = mdata.serialize();