//----------------------------------------------------------------
// Header section to make this work in ISTool
//----------------------------------------------------------------
#ifdef EXCLUDE
[Setup]

[_ISTool]
EnableISX=true
[Code]
#endif

// File Gaurds
#ifndef GUID_ISS
#define GUID_ISS

type
  TGUID = record
    D1: LongWord;
    D2: Word;
    D3: Word;
    D4: array[0..7] of Byte;
  end;

function CoCreateGuid(var Guid:TGuid):integer;
 external 'CoCreateGuid@ole32.dll stdcall';

function inttohex(l:longword; digits:integer):string;
var hexchars:string;
begin
 hexchars:='0123456789ABCDEF';
 setlength(result,digits);
 while (digits>0) do begin
  result[digits]:=hexchars[l mod 16+1];
  l:=l div 16;
  digits:=digits-1;
 end;
end;

function GetGuid(dummy:string):string;
var Guid:TGuid;
begin
  if CoCreateGuid(Guid)=0 then begin
  result:='{'+IntToHex(Guid.D1,8)+'-'+
           IntToHex(Guid.D2,4)+'-'+
           IntToHex(Guid.D3,4)+'-'+
           IntToHex(Guid.D4[0],2)+IntToHex(Guid.D4[1],2)+'-'+
           IntToHex(Guid.D4[2],2)+IntToHex(Guid.D4[3],2)+
           IntToHex(Guid.D4[4],2)+IntToHex(Guid.D4[5],2)+
           IntToHex(Guid.D4[6],2)+IntToHex(Guid.D4[7],2)+
           '}';
  end else
    result:='{00000000-0000-0000-0000-000000000000}';
end;

#endif
