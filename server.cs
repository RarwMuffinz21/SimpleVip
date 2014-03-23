// Made by RARWMUFFINZ, please add me as VIP on your server if you like my mod :D

function checkVip(%ID)
{
   %ID = mFloor(%ID);
   %file = new FileObject();
   %file.openForRead("Config/server/VipList.cs");
   %line = %file.readLine();
   for(%i=0;%i<getWordCount(%line);%i++)
      if(getWord(%line,%i) == %ID)
      {
         if(isObject(%p=findclientbyBL_ID(%ID)))
         {
            if(!%p.isVIP)
               messageall('',"\c4" @ %p.getPlayerName() @ " has become VIP (Auto)");
            %p.isVIP = true;
            %p.clanPrefix = $VIPMOD::VIPtag;
         }
         return true;
      }
      else
         return false;
   %file.close();
   %file.delete();
}

function VipTagUpdate()
	{	
		%inFileHandle = new FileObject() ;
		
		%inFileHandle.openForRead("Config/Server/viplist.cs") ;
	
		while(!%inFileHandle.IsEOF())
		{
			%inLine = %inFileHandle.readLine() ;
			
	                // output read text to console.
			findclientbyBL_ID(%inLine).clanPrefix = $VIPMod::Viptag;
		}
	
		%inFileHandle.close() ;
		%inFileHandle.delete();
	}

function servercmdSetVipTag(%client, %tag)
{
	echo("function called");
	if(%client.isAdmin)
	{
	echo("is admin");
		if(%tag $= "")
		{
		}
		else{
			$VIPMod::VIPtag = %tag;
			messageClient(%client,'',"\c4You have set the vip tag to: " @ $VIPMod::VIPtag);
			VipTagUpdate();
			}	
}
}
function servercmdaddVip(%client, %ID)
{
	if(%client.isAdmin)
	{
		if(!findclientbyBL_ID(%ID)==0)
		{
			addVip(%ID);
		}
		else
		{
		messageClient(%client,'',"No ID Specified or Player does not exist. Useage: /addVip [BLID] ");
		}

	}
	else
	{
	echo(%client.bl_id @ " tried to add a vip");
	}
}
function addVip(%ID)
{
   %ID = mFloor(%ID);
   if(checkVip(%ID))
      return 0; //If it exists, don't add it again
   %file = new FileObject();
   %file.openForAppend("Config/server/VipList.cs");
   %file.writeLine(" " @ %ID);
   if(isObject(%p=findclientbyBL_ID(%ID)))
   {
      if(!%p.isVIP)
         messageall('',"\c4" @ %p.getPlayerName() @ " has become VIP (Auto)");
      %p.isVIP = true;
      %p.clanPrefix = $VIPMod::VIPtag;
   }
   %file.close();
   %file.delete();
}


package  VipChecky 
{
   function GameConnection::autoAdminCheck(%this)
   {
      if($VIPMod::VIPTag $= "")
	{
		$VIPMod::VIPtag = "\c4[VIP]";
	}
      checkVip(%this);
      return parent::autoAdminCheck(%this);
   }
};
activatePackage(VipChecky);

// Events
registerOutputEvent(fxDTSBrick, "checkVip","",1);
registerInputEvent(fxDTSBrick, onVipTrue, "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent(fxDTSBrick, onVipFalse, "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

function fxDTSBrick::checkVip(%this,%client)
{
	echo("event Called");
   if(%client.isVip)
   {
	echo("Return True");
      %this.processInputEvent("onVipTrue", %client); //Call it if it is true
   }
   else
   {
	echo("return false");
      %this.processInputEvent("onVipFalse", %client);
   }
}

function fxDTSBrick::onVipTrue(%this,%client)
{
   $InputTarget_["Self"] = %this; //Call the brick
   $InputTarget_["Player"] = %client.player; //Call the object that is activating it
   $InputTarget_["Client"] = %client; //Call the object's client
   $InputTarget_["MiniGame"] = getMiniGameFromObject(%client); //Call the object's minigame
   %this.processInputEvent("onVipTrue", %client); //Process it
}

function fxDTSBrick::onVipFalse(%this,%client)
{
   $InputTarget_["Self"] = %this; //Call the brick
   $InputTarget_["Player"] = %client.player; //Call the object that is activating it
   $InputTarget_["Client"] = %client; //Call the object's client
   $InputTarget_["MiniGame"] = getMiniGameFromObject(%client); //Call the object's minigame
   %this.processInputEvent("onVipFalse", %client); //Process it
}