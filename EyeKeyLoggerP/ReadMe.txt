﻿
/* *************************************************************************** *
 *  Title:   EyeDentica Keylogger
 *  Authors: MF,MR,OS,ID,MB
 * 
 *  Summary: 
 *	EyeDentica Keylogger is a keyboard operation recordr for a non-hacking porpuse.
 *  This mecanizem ment to record the user keyboard and mouse usage only.
 * 
 * Usage:
 * You have several args you can pass to customize the
 * program's execution.
 * netLogger.exe -f [filename] -m [mode] -i [interval] -o [output]
 *		-f [filename](Name of the file. ".log" will always be the ext.)
 *		-m ['hour' or 'day'] saves logfile name appended by the hour or day.
 *		-i [interval] in milliseconds, flushes the buffer to either the
 *					  console or file. Shorter time = more cpu usage.
 *					  10000=10seconds : 60000=1minute : etc...
 *		-o ['file' or 'console' or 'service'] Outputs all data to either a file or console.
 * *************************************************************************** */