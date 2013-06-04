// Taken from http://sqldev.net

#define STRICT

#ifndef UNICODE
#define UNICODE
#endif

#ifndef _UNICODE
#define _UNICODE
#endif

#define	MAJORVERSION	1
#define	MINORVERSION	0
#define	BUILDNUMBER		2

#define	BUFFER	30000

#include <windows.h>
#include <tchar.h>
#include <stdio.h>

#include "sql.h"
#include "sqlext.h"
#include "odbcss.h"

#pragma warning (disable: 4996) // deprecated

extern "C" __declspec(dllexport) BOOL SqlEnumServers(_TCHAR* servers, int maxCount);

void PrintODBCError(SQLSMALLINT HandleType, SQLHANDLE hHandle);
BOOL ParseAndDisplay(LPTSTR lpFetchBuf, _TCHAR* servers, int maxCount);

const TCHAR szConnectStr[] = _T("Driver={SQL Server}");

#define DBFAILED(rc, htype, handle)			\
if (rc != SQL_SUCCESS)						\
{											\
	/*PrintODBCError(htype, handle);*/   	\
	if (rc == SQL_ERROR)					\
	{										\
		__leave;							\
	}										\
}											\

extern "C" BOOL SqlEnumServers(_TCHAR* servers, int maxCount)
{
	BOOL		iRet		= FALSE;	        // program return value
	LPTSTR		lpszSvr		= NULL;				// server name if not listing all servers
	LPTSTR		lpFetchBuf	= NULL;				// fetch buffer of size BUFFER

	SQLHENV		henv		= SQL_NULL_HENV;	// ODBC environment handle
	SQLHDBC		hdbc		= SQL_NULL_HDBC;	// ODBC database handle
	RETCODE		rc			= SQL_SUCCESS;		// ODBC return code

	SQLSMALLINT	nReqBufSize	= 0;
	SQLSMALLINT	nFetchBuf	= BUFFER;

    _sntprintf(servers, maxCount, _T("\0"));

	__try
	{
		// allocate ODBC enviroment handle
		//
		if (SQLAllocHandle(SQL_HANDLE_ENV, NULL, &henv) == SQL_ERROR)
		{	
			_sntprintf(servers, maxCount, _T("SQLAllocHandle(SQL_HANDLE_ENV) failed."));
			__leave;
		}

		// use ODBC 3.0 functionality instead of 2.0
		//
		rc = SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER)SQL_OV_ODBC3, 0);
		DBFAILED(rc, SQL_HANDLE_ENV, henv);

		// allocate database handle
		//
		rc = SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);
		DBFAILED(rc, SQL_HANDLE_ENV, henv);

		// set connection attribute to include extended information if requested, default is names only
		//
		rc = SQLSetConnectAttr(hdbc, SQL_COPT_SS_BROWSE_CONNECT, 
			(SQLPOINTER)(SQL_MORE_INFO_YES), SQL_IS_UINTEGER);
		DBFAILED(rc, SQL_HANDLE_DBC, hdbc);

		// Set the special attribute for large domain SQLBrowseConnect
		rc = SQLSetConnectAttr(hdbc, SQL_COPT_SS_BROWSE_CACHE_DATA, (SQLPOINTER) SQL_CACHE_DATA_YES, SQL_IS_INTEGER);
		DBFAILED(rc, SQL_HANDLE_DBC, hdbc);

		// fetch buffer is static of size
		//
		lpFetchBuf = new TCHAR[nFetchBuf];
		if (lpFetchBuf == NULL)
		{
			_sntprintf(servers, maxCount, _T("Not enough memory."));
			__leave;
		}

		rc = SQLBrowseConnect(
			hdbc, 
			(SQLTCHAR*)szConnectStr,
			SQL_NTS, 
			(SQLTCHAR*)lpFetchBuf, 
			nFetchBuf, 
			&nReqBufSize);
		DBFAILED(rc, SQL_HANDLE_DBC, hdbc);

        iRet = TRUE;

		while (SQL_NEED_DATA == rc && nReqBufSize > nFetchBuf)
		{
			ParseAndDisplay(lpFetchBuf, servers, maxCount);

			rc = SQLBrowseConnect(
				hdbc, 
				(SQLTCHAR*)szConnectStr,
				SQL_NTS, 
				(SQLTCHAR*)lpFetchBuf, 
				nFetchBuf, 
				&nReqBufSize);
			DBFAILED(rc, SQL_HANDLE_DBC, hdbc);
		}

		if (SQL_ERROR != rc)
		{
			ParseAndDisplay(lpFetchBuf, servers, maxCount);
		}
	}
	__finally
	{
		if (lpszSvr)
		{
			delete [] lpszSvr;
			lpszSvr = NULL;
		}

		if (lpFetchBuf)
		{
			delete [] lpFetchBuf;
			lpFetchBuf = NULL;
		}

		if (hdbc)
		{
			SQLDisconnect(hdbc);
			SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
			hdbc = SQL_NULL_HDBC;
		}

		if (henv)
		{
			SQLFreeHandle(SQL_HANDLE_ENV, henv);
			henv = SQL_NULL_HENV;
		}
	}

	return(iRet);
}

void PrintODBCError(SQLSMALLINT HandleType, SQLHANDLE hHandle)
{
	INT		i;
	INT		j;
	INT		msgstate;
	INT		severity;
	SDWORD	NativeErr;
	TCHAR	szSQLState[SQL_SQLSTATE_SIZE + 1];
	TCHAR   szMsg     [1024 + 1];
	TCHAR   ServerName[SQL_MAX_SQLSERVERNAME + 1];
	TCHAR   ProcName  [SQL_MAX_SQLSERVERNAME + 1];

	DBUSMALLINT LineNumber;

	if (SQLGetDiagField(
		HandleType, 
		hHandle, 
		0, 
		SQL_DIAG_NUMBER, 
		&i,
		sizeof(i), 
		NULL) == SQL_ERROR )
	{
		OutputDebugString(_T("SQLGetDiagField failed\n"));
	}
	else
	{
		for (j = 1; j <= i; j++)
		{
			if (SQLGetDiagRec(
				HandleType, 
				hHandle, 
				(SQLSMALLINT)j,
				(SQLTCHAR*)szSQLState,	
				&NativeErr, 
				(SQLTCHAR*)szMsg, 
				sizeof(szMsg) / sizeof(TCHAR),
				NULL) == SQL_ERROR )
			{
				OutputDebugString(_T("SQLGetDiagRec failed\n"));
			}
			else
			{	
				//	Get driver specific diagnostic fields
				//
				if (SQLGetDiagField(
					HandleType, 
					hHandle, 
					(SQLSMALLINT)j,
					SQL_DIAG_SS_MSGSTATE, 
					&msgstate, 
					SQL_IS_INTEGER,
					NULL) == SQL_ERROR )
				{
					msgstate = 0;
				}

				if (SQLGetDiagField(
					HandleType, 
					hHandle, 
					(SQLSMALLINT)j,
					SQL_DIAG_SS_SEVERITY, 
					&severity, 
					SQL_IS_INTEGER,
					NULL) == SQL_ERROR )
				{
					severity = 0;
				}

				if (SQLGetDiagField(
					HandleType, 
					hHandle, 
					(SQLSMALLINT)j,
					SQL_DIAG_SS_SRVNAME, 
					&ServerName, 
					sizeof(ServerName),
					NULL) == SQL_ERROR )
				{
					*ServerName = 0;
				}

				if (SQLGetDiagField(
					HandleType, 
					hHandle, 
					(SQLSMALLINT)j,
					SQL_DIAG_SS_PROCNAME, 
					&ProcName, 
					sizeof(ProcName),
					NULL) == SQL_ERROR )
				{
					*ProcName = 0;
				}

				if (SQLGetDiagField(
					HandleType, 
					hHandle, 
					(SQLSMALLINT)j,
					SQL_DIAG_SS_LINE, 
					&LineNumber, 
					SQL_IS_SMALLINT,
					NULL) == SQL_ERROR )
				{
					LineNumber = 0;
				}
			}
		} // for( j = 1; j <= i; j++ )
	}
}

BOOL ParseAndDisplay(LPTSTR lpFetchBuf, _TCHAR* servers, int maxCount)
{
	LPTSTR	lpBuf	= lpFetchBuf;
	LPTSTR	lpStart = lpFetchBuf;
	size_t	nSize	= 0;

	// position at start position in buffer
	//
	lpBuf = _tcschr(lpFetchBuf, '{');
	
	// character not found
	//
	if (NULL == lpBuf) 
		return FALSE;
	else
		lpBuf++;

	nSize = _tcslen(lpBuf);
	lpStart = lpBuf;

	for (size_t i = 0; i < nSize; i++, lpBuf++)
	{
		if (*lpBuf == ',' || *lpBuf == '}')
		{
			*lpBuf = '\0';

            if (maxCount - _tcslen(servers) > _tcslen(lpStart))
            {
                if (_tcslen(servers) != 0)
                {
                    _tcscat(servers, _T("\n"));
                }

                _tcscat(servers, lpStart);
            }

			i++;
			lpBuf++;

			lpStart = lpBuf;
		}
	}

	return TRUE;
}
